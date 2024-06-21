using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.LucidEditor;

namespace Cainos.PixelArtPlatformer_VillageProps
{
    public class Chest : MonoBehaviour
    {
        [FoldoutGroup("Reference")]
        public Animator animator;

        [FoldoutGroup("Reference")]
        public GameObject itemPrefab;  // Prefab của vật phẩm

        [FoldoutGroup("Reference")]
        public GameObject itemPrefab1;  // Prefab của vật phẩm

        [FoldoutGroup("Reference")]
        public GameObject monsterPrefab;  // Prefab của quái vật

        [FoldoutGroup("Reference")]
        public GameObject monsterPrefab1;  // Prefab của quái vật

        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;
                animator.SetBool("IsOpened", isOpened);
            }
        }
        private bool isOpened;

        [FoldoutGroup("Runtime"), Button("Open"), HorizontalGroup("Runtime/Button")]
        public void Open()
        {
            IsOpened = true;
            GenerateItemOrMonster();
        }

        [FoldoutGroup("Runtime"), Button("Close"), HorizontalGroup("Runtime/Button")]
        public void Close()
        {
            IsOpened = false;
        }

        private void Update()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f); // Kiểm tra xem có bất kỳ collider nào trong vùng hình tròn có bán kính 1f không
            bool playerNearby = false;
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Player")) // Kiểm tra xem collider có là của player không
                {
                    playerNearby = true;
                    break;
                }
            }

            if (playerNearby && Input.GetKeyDown(KeyCode.F) && !IsOpened)
            {
                Open();
            }
        }

        private void GenerateItemOrMonster()
        {
            bool generateMonster = Random.value > 0.5f; // 50% cơ hội để tạo ra quái vật

            if (generateMonster)
            {
                // Tạo ra quái vật
                if (Random.value > 0.5f) // Chọn một trong hai loại quái vật
                {
                    Instantiate(monsterPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(monsterPrefab1, transform.position, Quaternion.identity);
                }
            }
            else
            {
                // Tạo ra vật phẩm
                if (Random.value > 0.5f) // Chọn một trong hai loại vật phẩm
                {
                    Instantiate(itemPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(itemPrefab1, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
