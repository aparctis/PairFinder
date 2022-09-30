using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        //SCORE
        //then less time was needed to find a pair - then higher score
        private int curentScore = 0;
        [SerializeField] private TextMeshProUGUI scoreText;

        private bool scoreTimerStart = false;
        [SerializeField] private int maxScoreForPair = 10;
        [SerializeField] private int minScoreForPair = 1;

        [SerializeField] private float secondsToLoseScorePoint = 0.25f;
        private float scoreTime = 0;

        private int curentScoreForPair;


        #region Unity Events
        void Start()
        {
            InitGameField();
            ScoreUpdate(0);
        }

        void Update()
        {
            if (scoreTimerStart)
            {
                ScoreTimer();
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


        //Checking chosen sprite
        public void CheckMe(SpriteController sprite, int index)
        {
            if (checkingSprite == null)
            {
                checkingSprite = sprite;
                checkingIndex = index;
                scoreTimerStart = true;
            }

            else
            {
                clickAble = false;

                if (checkingIndex == index)
                {
                    //remove sprites
                    checkingSprite.Invoke("RemoveSprite", 0.5f);
                    sprite.Invoke("RemoveSprite", 0.5f);
                    //add score for pair and reload timer
                    ScoreUpdate(curentScoreForPair);
                    DropScoreTimer();
                    //update active pair count 
                    imagessInGame--;
                    if (imagessInGame == 0)
                    {
                        Invoke("InitGameField", 0.5f);
                    }
                }

                else
                {
                    //close sprites
                    checkingSprite.Invoke("CloseSprite", 0.5f);
                    sprite.Invoke("CloseSprite", 0.5f);
                    //reload score timer
                    DropScoreTimer();

                }
                checkingSprite = null;
            }
        }

        //Score
        private void ScoreUpdate(int addedScore)
        {
            curentScore += addedScore;
            scoreText.text = ("Score: " + curentScore);
        }
        private void ScoreTimer()
        {
            scoreTime += Time.deltaTime;
            if (scoreTime >= secondsToLoseScorePoint)
            {
                if (curentScoreForPair > minScoreForPair) curentScoreForPair--;
                scoreTime = 0;
            }
        }
        private void DropScoreTimer()
        {
            scoreTimerStart = false;
            curentScoreForPair = maxScoreForPair;
            scoreTime = 0;
        }

    }
}
