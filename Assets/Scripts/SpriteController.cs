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

        private int spriteIndex;
        private bool isOpen = false;

        [SerializeField] private GameField gameField;

        //visual
        [SerializeField] private bool useParticles;
        [SerializeField] private GameObject particles;


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
            if(useParticles) particles.SetActive(false);

            ren.sprite = backSprite;
            isOpen = true;
            gameField.CheckMe(this, spriteIndex);

        }

        public void CloseSprite()
        {
            if (useParticles) particles.SetActive(false);

            ren.sprite = frontSprite;
            isOpen = false;

            if (!gameField.clickAble) gameField.clickAble = true;
        }

        public void RemoveSprite()
        {
            if (useParticles) particles.SetActive(true);

            ren.sprite = frontSprite;
            isOpen = false;
            gameObject.SetActive(false);
            if (!gameField.clickAble) gameField.clickAble = true;

        }

    }
}
