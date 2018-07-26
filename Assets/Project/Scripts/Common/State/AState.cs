using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AState {
    public const string ONBEFORESTRING = "onbefore";
    public const string ONAFTERSTRING = "onafter";
    public const string ONLEAVESTRING = "onleave";
    public const string ONENTERSTRING = "onenter";
    public const string ONUPDATESTRING = "onupdate";
    public const string ONFIXEDUPDATESTRING = "onfixedupdate";

    public SMEvent smEvent;
    public StateMachine statemachine;

    public virtual bool OnBefore(SMEvent smEvent) { return true; }
    public virtual bool OnAfter(SMEvent smEvent) { return true; }
    public virtual bool OnLeave(SMEvent smEvent) { return true; }
    public virtual bool OnEnter(SMEvent smEvent) { return true; }
    public virtual bool OnUpdate(SMEvent smEvent) { return true; }
    public virtual bool OnFixedUpdate(SMEvent smEvent) { return true; }

    //public AState(StateMachine _stateMachine, SMEvent _smEvent) {
    //    statemachine = _stateMachine;
    //    smEvent = _smEvent;
    //}

    public void SetEventArray(ref Dictionary<string, Func<SMEvent, bool>> callbacks) {
        Func<SMEvent, bool> onbefore = x => OnBefore(x);
        Func<SMEvent, bool> onafter = x => OnAfter(x);
        Func<SMEvent, bool> onleave = x => OnLeave(x);
        Func<SMEvent, bool> onenter = x => OnEnter(x);
        Func<SMEvent, bool> onupdate = x => OnUpdate(x);
        Func<SMEvent, bool> onfixedupdate = x => OnFixedUpdate(x);

        callbacks.Add(ONBEFORESTRING + smEvent.Name, onbefore);
        callbacks.Add(ONAFTERSTRING + smEvent.Name, onafter);
        callbacks.Add(ONLEAVESTRING + smEvent.Name, onleave);
        callbacks.Add(ONENTERSTRING + smEvent.Name, onenter);
        callbacks.Add(ONUPDATESTRING + smEvent.Name, onupdate);
        callbacks.Add(ONFIXEDUPDATESTRING + smEvent.Name, onfixedupdate);
    }
}
