using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public const int gridRows = 2;
    public const int gridColumns = 4;
    public const float offsetX = 4f;
    public const float offsetY = 5f;

    [SerializeField] private Memory_Card originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh scoreLabel;
    public Memory_Card _firstRevealed;
    public Memory_Card _secondRevealed;

    private void Start()
    {
        scoreLabel.text = "Score: " + score;

        Vector3 startPos = originalCard.transform.position;

        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 };
        numbers = ShuffleArray(numbers);

        for (int i = 0; i < gridColumns; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                Memory_Card card;
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard);
                }

                int index = j * gridColumns + i;
                int id = numbers[index];
                card.SetCard(id, images[id]);//creates a random card

                float posX = (offsetX * i) + startPos.x;
                float posY = (offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    public bool canReveal
    {
        get { return _secondRevealed == null; }
    }

    private int score = 0;
    public void cardRevealed(Memory_Card card)
    {
        if(_firstRevealed == null)
        {
            _firstRevealed = card;
            Debug.Log(_firstRevealed.id);
        }
        else
        {
            _secondRevealed = card;
            Debug.Log(_secondRevealed.id);
            StartCoroutine(CheckMatch());
        }
    }
    
    private IEnumerator CheckMatch()
    {
        if(_firstRevealed.id == _secondRevealed.id)
        {
            score++;
            scoreLabel.text = "Score: " + score;
        }
        else
        {
            yield return new WaitForSeconds(.5f);

            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
        }

        _firstRevealed = null;
        _secondRevealed = null;
    }

    public static void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
