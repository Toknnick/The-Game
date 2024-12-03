using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineSweeper : MonoBehaviour
{
    [SerializeField] private Balancer balancer;
    [SerializeField] private MainManager mainManager;
    private int gridWidth;
    private int gridHeight;
    private int mineCount;
    private int remainingLives;

    public GameObject cellPrefab;
    public Transform gridParent;
    public TextMeshProUGUI livesText;

    [SerializeField] private GameObject panel;
    [SerializeField] private LostPanel EndGamePanel;

    private List<GameTitle> cells;
    private int cellsToReveal;
    private bool gameEnded;

    public void StartGame()
    {
        panel.SetActive(true);
        gridWidth = balancer.gridWidth;
        gridHeight = balancer.gridHeight;
        mineCount = balancer.mineCount;
        remainingLives = balancer.lives;
        mainManager.OffCells();

        InitializeGrid();
        PlaceMines();
        livesText.text = "Готов собирать мед!";
    }

    void InitializeGrid()
    {
        // Удаляем все существующие плитки из gridParent
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        cells = new List<GameTitle>();
        cellsToReveal = gridWidth * gridHeight - mineCount;

        // Устанавливаем правильный размер Grid Layout Group
        GridLayoutGroup gridLayout = gridParent.GetComponent<GridLayoutGroup>();
        if (gridLayout != null)
        {
            RectTransform parentRect = gridParent.GetComponent<RectTransform>();

            float cellWidth = parentRect.rect.width / gridWidth;
            float cellHeight = parentRect.rect.height / gridHeight;
            float cellSize = Mathf.Min(cellWidth, cellHeight); // Выбираем минимальное значение для квадратных ячеек

            gridLayout.cellSize = new Vector2(cellSize, cellSize);
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = gridWidth;
        }

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GameObject cellObj = Instantiate(cellPrefab, gridParent);
                GameTitle cell = cellObj.GetComponent<GameTitle>();
                cell.Initialize();
                cell.button.onClick.AddListener(() => OnCellClicked(cell));
                cells.Add(cell);
            }
        }
    }

    void PlaceMines()
    {
        for (int i = 0; i < mineCount; i++)
        {
            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, cells.Count);
            }
            while (cells[randomIndex].hasMine);

            cells[randomIndex].hasMine = true;
        }
    }

    void OnCellClicked(GameTitle cell)
    {
        if (gameEnded || cell.isRevealed) return;

        cell.isRevealed = true;

        if (cell.hasMine)
        {
            cell.Reveal();
            remainingLives--;
            livesText.text = $"Могу ведержать укусов: {remainingLives}";

            if (remainingLives <= 0)
            {
                EndGame(false);
                return;
            }
        }
        else
        {
            int nearbyMines = CountNearbyMines(cell);
            cell.SetMineCount(nearbyMines);
            cellsToReveal--;

            if (nearbyMines == 0)
            {
                RevealAdjacentCells(cell);
            }

            if (cellsToReveal <= 0)
            {
                EndGame(true);
                return;
            }
        }
    }

    int CountNearbyMines(GameTitle cell)
    {
        int mineCount = 0;
        Vector2Int cellPosition = GetCellPosition(cell);

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector2Int neighborPos = cellPosition + new Vector2Int(x, y);

                if (IsWithinBounds(neighborPos))
                {
                    GameTitle neighbor = GetCellAt(neighborPos);
                    if (neighbor.hasMine)
                    {
                        mineCount++;
                    }
                }
            }
        }

        return mineCount;
    }

    void RevealAdjacentCells(GameTitle cell)
    {
        Vector2Int cellPosition = GetCellPosition(cell);

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0) continue;

                Vector2Int neighborPos = cellPosition + new Vector2Int(x, y);

                if (IsWithinBounds(neighborPos))
                {
                    GameTitle neighbor = GetCellAt(neighborPos);

                    if (!neighbor.isRevealed && !neighbor.hasMine)
                    {
                        OnCellClicked(neighbor);
                    }
                }
            }
        }
    }

    Vector2Int GetCellPosition(GameTitle cell)
    {
        int index = cells.IndexOf(cell);
        return new Vector2Int(index % gridWidth, index / gridWidth);
    }

    bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < gridWidth && position.y < gridHeight;
    }

    GameTitle GetCellAt(Vector2Int position)
    {
        int index = position.y * gridWidth + position.x;
        return cells[index];
    }

    void EndGame(bool won)
    {
        mainManager.OnCells();

        gameEnded = true;

        foreach (var cell in cells)
        {
            if (cell.hasMine)
            {
                cell.Reveal();
            }
        }

        float gold = balancer.goldForGame;

        if (remainingLives == 2)
        {
            gold = gold * 0.7f;
        }
        if (remainingLives == 1)
        {
            gold = gold * 0.3f;
        }

        panel.SetActive(false);
        EndGamePanel.EndGame(won,gold);
        mainManager.EndTneGame(won, gold);
    }
}
