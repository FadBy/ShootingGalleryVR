using UnityEngine;

public class Target : MonoBehaviour
{
    public int points = 100;

    public void Hit()
    {
        Debug.Log("🎯 Попадание в мишень!");
        GameManager.Instance.AddScore(points);

        // Можно добавить реакцию мишени (падение, исчезновение и т.д.)
        //Destroy(gameObject); // временно просто уничтожаем
    }
}
