using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    [SerializeField] private GameObject _healthIcon;
    [SerializeField] private GameObject _healthRoot;
    [SerializeField] private TextMeshProUGUI _attackPowerText;
    private List<GameObject> _healthList = new List<GameObject>();

    private void Awake()
    {
        PlayerMovement.OnPlayerHPChanged += UpdateHealthUI;
        PlayerMovement.OnPlayerAPChanged += UpdateAPUI;
    }

    public void UpdateHealthUI(int HP)
    {
        Debug.Log("Change HP");
        StopAllCoroutines();
        StartCoroutine(UpdateUI(HP));
    }

    private IEnumerator UpdateUI(int HP)
    {
        while (_healthList.Count != HP)
        {
            Debug.Log("Changing HP");
            if (_healthList.Count < HP)
            {
                _healthList.Add(Instantiate(_healthIcon, _healthRoot.transform));
            }
            else if (_healthList.Count > HP)
            {
                Destroy(_healthList[0]);
                _healthList.RemoveAt(0);
            }
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Done Updating UI");
    }

    private void UpdateAPUI(int AP)
    {
        _attackPowerText.text = $"+{AP}";
    }
}
