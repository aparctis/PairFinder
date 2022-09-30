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

        //visual part
        [SerializeField] private float waitToClose;
        [SerializeField] private float waitToRemove;
        private float waitToRestart;


        //SCORE
        //then less time was needed to find a pair - then higher score
        private int curentScore = 0;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI recordText;

        private bool scoreTimerStart = false;
        [SerializeField] private int maxScoreForPair = 10;
        [SerializeField] private int minScoreForPair = 1;

        [SerializeField] private float secondsToLoseScorePoint = 0.25f;
        private float scoreTime = 0;

        private int curentScoreForPair;

        //record
        private bool recordAble = false;
        private int recordScore;
        [SerializeField] private GameObject particlesForRecord;


        //Sounds
        private AudioSource audio;
        private bool soundsInGame = true;

        [SerializeField] private AudioClip open_sound;
        [SerializeField] private AudioClip close_sound;
        [SerializeField] private AudioClip remove_sound;
        [SerializeField] private AudioClip record_sound;
        [SerializeField] private AudioClip newgame_sound;



        #region Unity Events
        void Start()
        {
            InitGameField();
            waitToRestart = waitToRemove + 1.0f;

            //download record
            recordScore = PlayerPrefs.GetInt("record");
            recordText.text = ("record: " + recordScore);

            //initialize audio
            audio = GetComponent<AudioSource>();

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

            recordAble = true;
            particlesForRecord.SetActive(false);

            curentScore = 0;
            ScoreUpdate(0);


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

                if (soundsInGame) PlayOpen();
            }

            else
            {
                clickAble = false;
                if (soundsInGame) PlayOpen();

                if (checkingIndex == index)
                {
                    //remove sprites and play sound
                    checkingSprite.Invoke("RemoveSprite", waitToRemove);
                    sprite.Invoke("RemoveSprite", waitToRemove);
                    if (soundsInGame) Invoke("PlayRemove", waitToRemove);
                    //add score for pair and reload timer
                    ScoreUpdate(curentScoreForPair);
                    DropScoreTimer();
                    //update active pair count 
                    imagessInGame--;
                    if (imagessInGame == 0)
                    {
                        Invoke("InitGameField", waitToRestart);
                        if (soundsInGame) Invoke("PlayNewGame", waitToRestart);
                        

                    }
                }

                else
                {
                    //close sprites and play sound
                    checkingSprite.Invoke("CloseSprite", waitToClose);
                    sprite.Invoke("CloseSprite", waitToClose);
                    if (soundsInGame) Invoke("PlayClose", waitToClose);
                   
                    //reload score timer
                    DropScoreTimer();

                }
                checkingSprite = null;
            }
        }


        #region Score and records
        //Score
        private void ScoreUpdate(int addedScore)
        {
            curentScore += addedScore;
            scoreText.text = ("Score: " + curentScore);

            //checking record
            if (curentScore > recordScore) NewRecord();


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
        
        //record
        private void NewRecord()
        {

            recordScore = curentScore;
            PlayerPrefs.SetInt("record", recordScore);
            recordText.text = ("record: " + recordScore);

            if (recordAble)
            {
                if (soundsInGame) PlayRecord();
                particlesForRecord.SetActive(true);
                recordAble = false;
            }

        }
        #endregion

        #region Sounds
        private void PlayOpen()
        {
            audio.clip = open_sound;
            audio.Play();
        }

        private void PlayClose()
        {
            audio.clip = close_sound;
            audio.Play();
        }

        private void PlayRemove()
        {
            audio.clip = remove_sound;
            audio.Play();
        }

        private void PlayNewGame()
        {
            audio.clip = newgame_sound;
            audio.Play();
        }
        private void PlayRecord()
        {
            audio.clip = record_sound;
            audio.Play();
        }

        public void SoundSwitch()
        {
            if (soundsInGame)
            {
                soundsInGame = false;
                
            }

            else
            {
                soundsInGame = true;
            }
        }
        #endregion
    }
}
