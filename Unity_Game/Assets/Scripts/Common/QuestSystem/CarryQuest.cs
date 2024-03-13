using System;
using System.Collections.Generic;
using Farm.Scripts.Interaction_System;
using Farm.Scripts.InteractionSystem;
using Town;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.QuestSystem
{
    public class CarryQuest : Quest
    {
        public Interactable.InteractableObject objectToBeCarried;
        public List<Interactable> itemsToBeCarried;
        public List<PlacementHint> placementHints;

        private int _hintIdx;

        public void Start()
        {
            if (itemsToBeCarried.Count != placementHints.Count)
            {
                Debug.LogError("Number of items to be carried are not equal with the number of placement hints");
            }

            _hintIdx = 0;
        }

        protected override void SetupQuest()
        {
            state = State.InProgress;
            itemsToBeCarried.ForEach(item => item.readyToInteract = true);
        }

        public override void CompleteQuest(Interactor interactor, NPC requester)
        {
            QuestManager.Instance.CompleteQuest();
        }

        protected override void CheckObjective()
        {
            if (currentAmount < requiredAmount) return;
            state = State.Completed;
        }
    
        public void OnObjectPickUp(Interactable.InteractableObject interactableObject)
        {
            if (interactableObject != objectToBeCarried) return;
            placementHints[_hintIdx].readyToInteract = true;
            placementHints[_hintIdx].gameObject.SetActive(true);
            _hintIdx++;
        }

        public void OnObjectPlaced(Interactable.InteractableObject interactableObject)
        {
            if (interactableObject != objectToBeCarried) return;
            currentAmount++;
            CheckObjective();
        }
    }
}
