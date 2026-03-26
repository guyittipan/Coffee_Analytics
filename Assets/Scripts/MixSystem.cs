using UnityEngine;

public class MixSystem : MonoBehaviour
{
    [SerializeField] private RecipeDatabase recipeDB;
    [SerializeField] private Board board;
    private Cell selectedA;
    private Cell selectedB;
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private ComboSystem comboSystem;
    [SerializeField] private TimerSystem timerSystem;   

    [Header("UI")]
    [SerializeField] private MixResultUI mixResultUI;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip burntCoffeeSound;
    [SerializeField] private AudioClip rerollSound;
    [SerializeField] private AudioClip goldenMilkSound; 

    [Header("Special Effects")]
    [SerializeField] private RerollEffectUI rerollEffectUI;   // 👈 เพิ่มตัวนี้
    [SerializeField] private TimeChangeEffectUI timeChangeEffectUI;   // 👈 เพิ่มอันนี้


    public void SelectCell(Cell cell)
    {
        if (cell == null || cell.CurrentIngredient == null)
            return;

        var type = cell.CurrentIngredient.Type;

        // 1) Burnt Coffee (debuff)
        if (type == IngredientType.BurntCoffee)
        {
            OnSelectBurntCoffee(cell);
            return;
        }

        // 2) Golden Milk (เพิ่มเวลา)
        if (type == IngredientType.GoldenMilk)
        {
            OnSelectGoldenMilk(cell);
            return;
        }

        // 3) Reroll Chocolate (สุ่มทั้งกระดานใหม่)
        if (type == IngredientType.RerollChocolate)
        {
            OnSelectRerollChocolate(cell);
            return;
        }

        // ------------------ เคสปกติ (ส่วนผสมจริง) ------------------
        if (selectedA == null)
        {
            selectedA = cell;
            return;
        }

        if (selectedB == null && cell != selectedA)
        {
            selectedB = cell;
            TryMix();
            return;
        }

        selectedA = selectedB;
        selectedB = cell;
        TryMix();
    }

    private void TryMix()
    {
        if (selectedA == null || selectedB == null) return;

        var ia = selectedA.CurrentIngredient?.Type ?? IngredientType.None;
        var ib = selectedB.CurrentIngredient?.Type ?? IngredientType.None;

        var recipe = recipeDB.GetRecipe(ia, ib);
        if (recipe != null)
{
    // เอาคูณคอมโบจาก ComboSystem
    float comboMultiplier = comboSystem != null ? comboSystem.UpdateCombo(recipe.recipeName) : 1f;
    int finalScore = Mathf.RoundToInt(recipe.baseScore * comboMultiplier);

    // ✅ ScoreSystem ต้องการ 2 ตัว เลยส่ง 0 ไปตัวที่สอง
    scoreSystem?.AddScore(finalScore, 0);

    // ✅ เก็บ analytics ว่าคอมโบนี้ถูกใช้
    ComboAnalytics.I?.RecordCombo(recipe.recipeName);

    // ลบของเก่า + เติมใหม่
    Destroy(selectedA.CurrentIngredient?.gameObject);
    Destroy(selectedB.CurrentIngredient?.gameObject);
    selectedA.Clear();
    selectedB.Clear();
    board.RefillEmpty();

            // เสียง
            if (audioSource != null && successSound != null)
                audioSource.PlayOneShot(successSound);

            // รูปเมนู
            if (mixResultUI != null)
                mixResultUI.Show(recipe.recipeName);
        }
        else
        {
            comboSystem?.ResetCombo();
        }

        selectedA = null;
        selectedB = null;
    }

    private void OnSelectBurntCoffee(Cell cell)
    {
        Debug.Log("Burnt Coffee clicked! Time debuff!");

        // 1) ลดเวลา 5 วินาที
        if (timerSystem != null)
        {
            timerSystem.ReduceTime(5f);
        }
        if (timeChangeEffectUI != null)
        {
            timeChangeEffectUI.ShowMinus();
        }

        // 2) รีเซ็ตคอมโบ
        if (comboSystem != null)
        {
            comboSystem.ResetCombo();
        }

        // 3) เขย่ากล้อง
        var shaker = FindObjectOfType<CameraShaker>();
        if (shaker != null)
        {
            shaker.Shake();
        }

        // 4) ลบ ingredient ออกจาก cell
        if (cell.CurrentIngredient != null)
        {
            Destroy(cell.CurrentIngredient.gameObject);
        }
        cell.Clear();

        // 5) เติมช่องว่าง
        if (board != null)
        {
            board.RefillEmpty();
        }

        // 6) รีเซ็ตตัวเลือก
        selectedA = null;
        selectedB = null;

        if (audioSource != null && burntCoffeeSound != null)
        {
            audioSource.PlayOneShot(burntCoffeeSound);
        }
    }

    private void OnSelectGoldenMilk(Cell cell)
    {
        Debug.Log("Golden Milk clicked! Time +5s");

        // 1) เพิ่มเวลา
        if (timerSystem != null)
        {
            timerSystem.AddTime(5f);   // ปรับจำนวนวินาทีได้ตามใจ
        }
        if (timeChangeEffectUI != null)
        {
            timeChangeEffectUI.ShowPlus();
        }
        // 2) เล่นเสียง Golden Milk
        if (audioSource != null && goldenMilkSound != null)
        {
            audioSource.PlayOneShot(goldenMilkSound);
        }

        // 3) กล้องสั่นเบา ๆ ให้รู้สึกดี
        var shaker = FindObjectOfType<CameraShaker>();
        if (shaker != null)
        {
            shaker.Shake(0.15f, 0.08f);   // สั่นเบา ๆ นุ่มกว่า Burnt Coffee
        }

        // 4) ลบ ingredient ออกจาก cell
        if (cell.CurrentIngredient != null)
        {
            Destroy(cell.CurrentIngredient.gameObject);
        }
        cell.Clear();

        // 5) เติมของใหม่ในช่องนี้
        if (board != null)
        {
            board.RefillEmpty();
        }

        // 6) รีเซ็ตตัวเลือก
        selectedA = null;
        selectedB = null;
    }

    private void OnSelectRerollChocolate(Cell cell)
    {
        Debug.Log("Reroll Chocolate clicked! Reroll all board!");

        // 1) เล่นเสียง reroll
        if (audioSource != null && rerollSound != null)
        {
            audioSource.PlayOneShot(rerollSound);
        }

        // 2) กล้องสั่นนิดหน่อย
        var shaker = FindObjectOfType<CameraShaker>();
        if (shaker != null)
        {
            shaker.Shake(0.2f, 0.1f);
        }

        // 3) เล่นเอฟเฟกต์ไอคอนหมุน ๆ เด้งขึ้นมา
        if (rerollEffectUI != null)
        {
            rerollEffectUI.Play();
        }

        // 4) ลบตัว reroll เองจาก cell
        if (cell.CurrentIngredient != null)
        {
            Destroy(cell.CurrentIngredient.gameObject);
        }
        cell.Clear();

        // 5) Reroll ทั้งกระดาน
        if (board != null)
        {
            board.RerollAll();
        }

        // 6) รีเซ็ตตัวเลือก A/B
        selectedA = null;
        selectedB = null;
    }
}
