using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Battleship
{
    public class GameManager : MonoBehaviour
    {

        //coord where player fired
        private bool[,] hits;

        //total number of rows and columns
        private int nRows;
        private int nCols;

        //current row and column
        private int row;
        private int col;

        //number of ships hit
        private int score;

        //runtime of the game
        private int time;

        //parent of cells
        [SerializeField] Transform gridRoot;

        //template used to populate grid
        [SerializeField] GameObject cellPrefab;
        [SerializeField] GameObject winLabel;
        [SerializeField] GameObject restart;
        [SerializeField] TextMeshProUGUI timeLabel;
        [SerializeField] TextMeshProUGUI scoreLabel;

        //2D array
        [SerializeField]
        private int[,] grid = new int[,]
        {

            {1,1,0,0,1},
            {0,0,0,0,0},
            {0,0,1,0,1},
            {1,0,1,0,0},
            {1,0,1,0,1}

        };

        private void Awake()
        {
            //initialize the grid
            nRows = grid.GetLength(0);
            nCols = grid.GetLength(1);

            //create identical 2D array that is a bool instead of an int
            hits = new bool[nRows, nCols];

            //populate the grid using a loop until it fills the grid
            for (int i = 0; i < nRows * nCols; i++)
            { 
            
                //create instance of cell prefab and child it to gridRoot
                Instantiate(cellPrefab, gridRoot);            
            
            }

            //select 0,0 cell on awake
            SelectCurrentCell();
            InvokeRepeating("IncrementTime", 1f, 1f);
            
        }

        Transform GetCurrentCell()
        {
            //find child index
            int index = (row * nCols) + col;

            //return child index            
            return gridRoot.GetChild(index);

        }

        void SelectCurrentCell()
        {

            Transform cell = GetCurrentCell();

            //set cursor position to current cell
            Transform cursor = cell.Find("Cursor");
            cursor.gameObject.SetActive(true);

        }


        void UnselectCurrentCell()
        {

            Transform cell = GetCurrentCell();

            //set cursor image to inactive for that cell
            Transform cursor = cell.Find("Cursor");
            cursor.gameObject.SetActive(false);

        }

        public void MoveHorizonal(int amt)
        {

            UnselectCurrentCell();

            col += amt;

            //restrict col to bounds of the grid
            col = Mathf.Clamp(col, 0, nCols - 1);

            SelectCurrentCell();

        }

        public void MoveVertonal(int amt) 
        {
        
            UnselectCurrentCell();

            row += amt;

            //restrict row to bounds of the grid
            row = Mathf.Clamp(row, 0, nRows - 1);

            SelectCurrentCell();
        
        }

        
        void ShowHit()
        {

            Transform cell = GetCurrentCell();

            //show hit image
            Transform hit = cell.Find("Hit");
            hit.gameObject.SetActive(true);

        }

        void ShowMiss()
        {

            Transform cell = GetCurrentCell();

            //show miss image
            Transform miss = cell.Find("Miss");
            miss.gameObject.SetActive(true);

        }

        void IncrementScore()
        {

            score++;

            //update score label
            scoreLabel.text = string.Format("{0}", score);

        }

        public void Fire()
        {
            //check if the current cell has been hit
            if (hits[row, col]) return;

            //mark cell as hit
            hits[row, col] = true;

            //check if cell has a ship and update score on hit
            if (grid[row, col] == 1)
            {
                ShowHit();
                IncrementScore();

            }
            else
            {
                ShowMiss();

            }

        }

        public void TryEndGame()
        {
            Debug.Log("TryEndGame");

            //check if all ships have been hit
            bool allShipsHit = true;
            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    //ignore if cell is not a ship
                    if (grid[row, col] == 0) continue;

                    //ignore if cell has not been hit
                    if (!hits[row, col])
                    {
                        allShipsHit = false;
                        break;
                    }
                }

                if (!allShipsHit) break;
            }

            //if all ships have been hit, show winLabel and stop timer
            if (allShipsHit)
            {
                winLabel.SetActive(true);
                restart.SetActive(true);
                CancelInvoke("IncrementTime");
            }
        }

        void IncrementTime()
        {

            time++;

            timeLabel.text = string.Format("{0}:{1}", time / 60, (time % 60).ToString("00"));

        }

        public void Restart()
        {

            UnselectCurrentCell();

            //reset row and column to 0,0
            row = 0;
            col = 0;

            SelectCurrentCell();

            //reset 2D array
            hits = new bool[nRows, nCols];

            //reset timer
            time = 0;
            timeLabel.text = "0:00";

            //reset score
            score = 0;
            scoreLabel.text = "0";

            //reset hit and miss objects on each cell
            for (int i = 0; i < gridRoot.childCount; i++)
            {
                Transform cell = gridRoot.GetChild(i);
                Transform hit = cell.Find("Hit");
                if (hit != null) hit.gameObject.SetActive(false);

                Transform miss = cell.Find("Miss");
                if (miss != null) miss.gameObject.SetActive(false);
            }

            //randomize ship positions
            for (int r = 0; r < nRows; r++)
            {
                for (int c = 0; c < nCols; c++)
                {
                    //random roll
                    int randomNumber = Random.Range(0, 11);

                    //set cell to 0 or 1
                    if (randomNumber > 5)
                        grid[r, c] = 1;
                    else
                        grid[r, c] = 0;
                }
            }

            //turn off winLabel and restart panel
            winLabel.SetActive(false);
            restart.SetActive(false);

        }


        // Start is called before the first frame update
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {
            TryEndGame();

        }
    }
}
