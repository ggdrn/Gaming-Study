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
            _targets.Add(target);
            // target.OnDestroyEvent += RemoveTarget;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Target target)) return;
        if (currentTarget == target) currentTarget = null;
        _targets.Remove(target);
    }

    public bool SelectTarget()
    {
        if (_targets.Count == 0) return false;
        if (currentTarget != null)
        {
            int currentIndex = _targets.IndexOf(currentTarget);
            if (currentIndex + 1 >= _targets.Count)
            {
                currentTarget = null;
                return false;
            }
            currentTarget = _targets[currentIndex + 1];
            return true;
        }
        currentTarget = _targets[0];
        return true;
    }
}