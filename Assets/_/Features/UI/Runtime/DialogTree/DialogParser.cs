using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


    public class DialogParser : MonoBehaviour
    {
        [SerializeField] private DialogContainer dialogue;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Button choicePrefab;
        [SerializeField] private Transform buttonContainer;

        private void Start()
        {
            var narrativeData = dialogue.m_nodeLinks.First(); //Entrypoint node
            ProceedToNarrative(narrativeData.m_targetNodeGuid);
        }

        private void ProceedToNarrative(string narrativeDataGUID)
        {
            var text = dialogue.m_dialogNodeData.Find(x => x.m_guid == narrativeDataGUID).m_dialogData.m_message;
            var choices = dialogue.m_nodeLinks.Where(x => x.m_baseNodeGuid == narrativeDataGUID);
            dialogueText.text = ProcessProperties(text);
            var buttons = buttonContainer.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
            }

            foreach (var choice in choices)
            {
                var button = Instantiate(choicePrefab, buttonContainer);
                button.GetComponentInChildren<TMP_Text>().text = ProcessProperties(choice.m_portName);
                button.onClick.AddListener(() => ProceedToNarrative(choice.m_targetNodeGuid));
            }
        }

        private string ProcessProperties(string text)
        {
            foreach (var exposedProperty in dialogue.m_exposedProperties)
            {
                text = text.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
            }
            return text;
        }
    }