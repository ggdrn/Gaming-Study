using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [field: SerializeField] public List<Target> _targets = new();
    [field: SerializeField] public Target currentTarget { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Target target))
        {
            if (!_targets.Contains(target))
            {
                _targets.Add(target);
                target.SetTargetVisual(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Target target)) return;
        if (currentTarget == target)
        {
            currentTarget = null;
        }
        target.SetTargetALock(false);
        target.SetTargetVisual(false);
        _targets.Remove(target);
    }

    public bool SelectTarget()
    {
        if (_targets.Count == 0)
        {
            currentTarget = null;
            return false;
        }
        if (currentTarget != null)
        {
            int currentIndex = _targets.IndexOf(currentTarget);
            if (currentIndex + 1 >= _targets.Count)
            {
                currentTarget.SetTargetALock(false);
                currentTarget.SetTargetVisual(true);
                currentTarget = null;
                return false;
            }
            currentTarget = _targets[currentIndex + 1];
        }
        else
        {
            currentTarget = _targets[0];
            currentTarget.SetTargetALock(true);
            currentTarget.SetTargetVisual(false);
        }
        return currentTarget != null;
    }
}