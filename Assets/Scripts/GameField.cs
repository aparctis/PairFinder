using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PairFinder
{
    public class GameField : MonoBehaviour
    {
        //Sprites and images, that we would use
        public List<SpriteController> all_sprites;
        public List<Sprite> all_images;
        
        //number of images in game(we can add more then 8 images and rest of them just wouldn`t be used)
        private int imagessInGame;


        List<Sprite> images_inGame = new List<Sprite>();
        List<SpriteController> sprites_inGame = new List<SpriteController>();

        private SpriteController checkingSprite = null;
        private int checkingIndex;
        public bool clickAble = true;

        #region Unity Events
        void Start()
        {
            InitGameField();

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                InitGameField();
            }
        }
        #endregion


        #region Randomize Game Field
        private void InitGameField()
        {
            RestAll();
            imagessInGame = all_sprites.Count / 2;

            //creating and shuffling new list of images
            List<Sprite> images_forShuffle = new List<Sprite>();
            CreateImages(all_images, images_forShuffle);
            ShuffleImages(images_forShuffle, imagessInGame);


            //creating and shuffling new list of sprites
            List<SpriteController> sprites_forShuffle = new List<SpriteController>();
            CreateSprites(all_sprites, sprites_forShuffle);
            ShuffleSprites(sprites_forShuffle, all_sprites.Count);

            //seting images for sprites, using shuffled lists
            SetImages(sprites_inGame, images_inGame, imagessInGame);

        }


        //Images
        private void CreateImages(List<Sprite> donorList, List<Sprite> list)
        {
            for (int i = 0; i < donorList.Count; i++)
            {
                list.Add(donorList[i]);

            }
        }

        private void ShuffleImages(List<Sprite> list, int imagesNeeded)
        {
            for (int i = 0; i < imagesNeeded; i++)
            {

                int imageInd = Random.Range(0, list.Count - 1);
                if (list[imageInd] != null)
                {
                    images_inGame.Add(list[imageInd]);
                    list.RemoveAt(imageInd);

                }


            }
        }


        //Sprites
        private void CreateSprites(List<SpriteController> donorList, List<SpriteController> list)
        {
            for (int i = 0; i < donorList.Count; i++)
            {
                list.Add(donorList[i]);

            }
        }

        private void ShuffleSprites(List<SpriteController> list, int spritesNeeded)
        {
            for (int i = 0; i < spritesNeeded; i++)
            {

                int imageInd = Random.Range(0, list.Count - 1);

                if (list[imageInd] != null)
                {
                    sprites_inGame.Add(list[imageInd]);
                    list.RemoveAt(imageInd);

                }


            }
        }

        private void SetImages(List<SpriteController> sprites, List<Sprite> images, int pair_count)
        {
            int spriteIndex = 0;

            for(int i = 0; i<pair_count; i++)
            {
                for(int x=0; x < 2; x++)
                {
                    sprites[spriteIndex].InitSprite(images[i], i);
                    spriteIndex++;
                }
            }
        }

        private void RestAll()
        {
            sprites_inGame.Clear();
            images_inGame.Clear();
            checkingSprite = null;
            foreach(SpriteController sprite in all_sprites)
            {
                sprite.gameObject.SetActive(true);
            }
            clickAble = true;

        }
        #endregion



        public void CheckMe(SpriteController sprite, int index)
        {
            if (checkingSprite == null)
            {
                checkingSprite = sprite;
                checkingIndex = index;
            }

            else
            {
                clickAble = false;

                if (checkingIndex == index)
                {
                    checkingSprite.Invoke("RemoveSprite", 0.5f);
                    sprite.Invoke("RemoveSprite", 0.5f);
                    imagessInGame--;
                    if (imagessInGame == 0)
                    {
                        Invoke("InitGameField", 0.5f);
                    }
                }

                else
                {
                    checkingSprite.Invoke("CloseSprite", 0.5f);
                    sprite.Invoke("CloseSprite", 0.5f);
                }
                checkingSprite = null;
            }
        }




    }
}
