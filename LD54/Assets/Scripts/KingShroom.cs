using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class KingShroom : Interactable
{
    [SerializeField] private Item _hpBoostItem;
    [SerializeField] private Item _apBoostItem;
    [SerializeField] private Item _fragrantFlower;
    [SerializeField] private Item _bearPelt;

    [Header("Dialogue")]
    [SerializeField] private Dialogue _dialogues;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private GameObject _shroomUI;
    [SerializeField] private float _textCooldown = 0.1f;
    private float _currentTextCooldown;


    private bool _canInteract = true;
    private List<string> _displayText = new List<string>();
    private int _currentTextIndex;
    private bool _writingText;
    private int _charIndex = 0;
    private int progress = 0;
    private bool _displayedImportantText = false;

    




    public UnityEvent OnFirstExpansion;
    public UnityEvent OnSecondExpansion;

    public override void OnInteract()
    {
        if (!_canInteract) return;
        InteractUI.SetActive(false);
        _canInteract = false;
        PlayerMovement.PlayerInstance.CanMove = false;
        _shroomUI.SetActive(true);

        switch (progress)
        {
            case 0:
                _displayText.Add(_dialogues.KeyDialogues[0]);
                _displayText.Add(_dialogues.InteractDialogue[0]);
                _currentTextIndex = 0;
                break;
            case 1:
                _displayText.Add(_dialogues.KeyDialogues[1]);
                _displayText.Add(_dialogues.InteractDialogue[1]);
                break;
            case 2:
                _displayText.Add(_dialogues.KeyDialogues[2]);
                break;
        }

        _dialogueText.text = string.Empty;
        _writingText = true;

        // Talks to player telling the player what it needs next
        // Player can choose to give items when the player has what it needs
    }
    private void WriteText()
    {
        _dialogueText.text += _displayText[_currentTextIndex][_charIndex];
        _charIndex++;

        if (_charIndex >= _displayText[_currentTextIndex].Length)
        {
            _writingText = false;
            _charIndex = 0;
        }


    }

    public override void Update()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(4, 4), 0);
        foreach(Collider2D col in hitColliders)
        {
            if(col.TryGetComponent(out ItemPickup newItem))
            {
                if (newItem.CanPickup)
                {
                    Debug.Log("Item Entered Trigger");
                    GiveItem(newItem.GetItem());
                    newItem.MoveToShroom(transform);
                }
            }
        }


        InteractUI.SetActive(PlayerInRange && _canInteract);

        _currentTextCooldown += Time.deltaTime;

        if(_writingText && _currentTextCooldown >= _textCooldown)
        {
            WriteText();
            _currentTextCooldown = 0;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_writingText)
            {
                Debug.Log("Quick Ending Text");
                _writingText = false;
                _charIndex = 0;
                _dialogueText.text = _displayText[_currentTextIndex];
            }
            else 
            {
                _currentTextIndex++;
                if (_displayText.Count > _currentTextIndex)
                {
                    Debug.Log("Continuing Text");
                    _dialogueText.text = string.Empty;
                    _writingText = true;
                }
                else
                {
                    Debug.Log("Stopping Text");
                    // stop talking to shroom
                    _displayText.Clear();
                    _charIndex = 0;
                    _dialogueText.text = string.Empty;
                    _currentTextIndex = 0;
                    _shroomUI.SetActive(false);
                    _canInteract = true;
                    PlayerMovement.PlayerInstance.CanMove = true;
                }

            }
           
        }
    }

    public void GiveItem(Item newItem)
    {
        if(newItem == _hpBoostItem)
        {
            PlayerMovement.PlayerInstance.IncreaseMaxHealth();

        } else if (newItem == _apBoostItem)
        {
            PlayerMovement.PlayerInstance.ChangeAttackPower(1);
        } else if (newItem == _fragrantFlower)
        {
            progress = 1;
            OnFirstExpansion.Invoke();
            transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            OnInteract();
        } else if (newItem == _bearPelt)
        {
            progress = 2;
            OnSecondExpansion.Invoke();
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            OnInteract();
        }
    }
}
