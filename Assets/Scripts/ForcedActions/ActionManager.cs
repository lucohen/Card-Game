using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private Stack<PendingAction> _pendingActions = new();
    public List<Card> markedCards;
    public GameObject confirmButton;

    public bool HasPendingActions => _pendingActions.Count > 0;
    public PendingAction Current => _pendingActions.TryPeek(out var a) ? a : null;

    public static ActionManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Push(PendingAction action) //move the action to the front of the stack, is now "Current"
    {
        _pendingActions.Push(action);
        action.OnActivated();
    }

    public void Resolve() // 
    {
        if (_pendingActions.Count == 0) return;
        var action = _pendingActions.Pop();
        action.OnResolved();
    }

    public void PressConfirmButton()
    {
        if (Current is DiscardAction discard && discard.CanConfirm)
        {
            Resolve();
        }
        else if (Current is ExileAction exile && exile.CanConfirm)
        {
            Resolve();
        }
    }
}
