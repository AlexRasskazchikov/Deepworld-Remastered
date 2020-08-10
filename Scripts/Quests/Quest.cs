using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    bool _is_quest_ended;
    [Header("Quest Meta Settings")]
    [SerializeField] string _QuestName;
    [SerializeField] [Multiline] string _QuestDescription;
    [SerializeField] List<Part> _QuestParts;

    TMPro.TextMeshProUGUI QuestDescriptionUI;
    TMPro.TextMeshProUGUI QuestNameUI;
    UnityEngine.UI.Image QuestIcon;
    public GameObject EndedQuestTemplate;

    Sprite QuestEndedIcon;
    string QuestEndedText;


    public string QuestDescription {
        get {return _QuestDescription;}
        set {_QuestDescription = value;}
    }

    public string QuestName {
        get {return _QuestName;}
        set {_QuestName = value;}
    }

    public List<Part> parts {
        get {return _QuestParts;}
    }

    public bool is_quest_ended {
        get {return _is_quest_ended;}
        set {_is_quest_ended = value;}
    }

    void Start() {
        QuestDescriptionUI = gameObject.transform.FindChild("QuestDescription").GetComponent<TMPro.TextMeshProUGUI>();
        QuestNameUI = gameObject.transform.FindChild("QuestName").GetComponent<TMPro.TextMeshProUGUI>();
        QuestIcon = gameObject.transform.FindChild("QuestImage").GetComponent<UnityEngine.UI.Image>();

        QuestDescriptionUI.text = QuestDescription;
        QuestNameUI.text = QuestName;

        QuestEndedIcon = EndedQuestTemplate.gameObject.transform.FindChild("QuestImage").GetComponent<UnityEngine.UI.Image>().sprite;
        QuestEndedText = EndedQuestTemplate.gameObject.transform.FindChild("QuestName").GetComponent<TMPro.TextMeshProUGUI>().text;
    }

    void Update() {
        if(Check()) {
            is_quest_ended = true;
            QuestDescriptionUI.text = QuestDescription;
            QuestNameUI.text = QuestEndedText;
            QuestIcon.sprite = QuestEndedIcon;
        }
    }

    bool Check() {
        foreach (var part in parts)
        {
            if (!part.is_part_ended) {
                return false;
            }
        }
        return true;
    }
}
