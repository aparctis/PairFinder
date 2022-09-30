using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PairFinder
{
    public class SpriteController : MonoBehaviour
    {
        private SpriteRenderer ren;
        [SerializeField] private Sprite backSprite;
        [SerializeField] private Sprite frontSprite;

        private bool isOpen = false;
        [SerializeField] private GameField gameField;


        public int spriteIndex;

        void Start()
        {
            ren = GetComponent<SpriteRenderer>();
        }

        private void OnMouseDown()
        {
            if (gameField.clickAble)
            {
                if (!isOpen)
                {
                    OpenSprite();

                }

            }


        }

        public void InitSprite(Sprite sprite, int index)
        {
            backSprite = sprite;
            spriteIndex = index;
        }


        //Sprite actions
        private void OpenSprite()
        {
            ren.sprite = backSprite;
            isOpen = true;
            gameField.CheckMe(this, spriteIndex);

        }

        public void CloseSprite()
        {
            ren.sprite = frontSprite;
            isOpen = false;

            if (!gameField.clickAble) gameField.clickAble = true;
        }

        public void RemoveSprite()
        {
            ren.sprite = frontSprite;
            gameObject.SetActive(false);
            if (!gameField.clickAble) gameField.clickAble = true;

        }

    }
}
