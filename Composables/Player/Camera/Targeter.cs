using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [field: SerializeField] public List<Target> _targets = new();
    [field: SerializeField] public Target currentTarget { get; private set; }
    public enum LockOnDirection
    {
        ToLeft,
        ToRight
    }


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

    void Update()
    {
        _targets.Sort((a, b) => Vector3.Distance(transform.position, a.transform.position)
                .CompareTo(Vector3.Distance(transform.position, b.transform.position)));
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

    public bool SwitchTarget(LockOnDirection direction, Camera cam)
    {
        if (currentTarget == null || _targets.Count <= 1 || cam == null) return false;
        Vector3 currentLocal = cam.transform.InverseTransformPoint(currentTarget.transform.position);
        Target bestCandidate = null;
        float bestDeltaX = float.MaxValue;
        foreach (var target in _targets)
        {
            if (target == currentTarget) continue;
            Vector3 targetLocal = cam.transform.InverseTransformPoint(target.transform.position);
            float deltaX = targetLocal.x - currentLocal.x;
            // LockOnToR → buscar alvo à DIREITA
            if (direction == LockOnDirection.ToRight && deltaX <= 0) continue;
            // LockOnToL → buscar alvo à ESQUERDA
            if (direction == LockOnDirection.ToLeft && deltaX >= 0) continue;
            float absDelta = Mathf.Abs(deltaX);
            if (absDelta < bestDeltaX)
            {
                bestDeltaX = absDelta;
                bestCandidate = target;
            }
        }
        if (bestCandidate == null) return false;
        SetCurrentTarget(bestCandidate);
        return true;
    }

private void SetCurrentTarget(Target newTarget)
{
    if (currentTarget != null)
    {
        currentTarget.SetTargetALock(false);
        currentTarget.SetTargetVisual(true);
    }

    currentTarget = newTarget;

    currentTarget.SetTargetALock(true);
    currentTarget.SetTargetVisual(false);
}


    public bool SelectTarget()
    {
        if (_targets.Count == 0)
        {
            currentTarget = null;
            return false;
        }

        // if (currentTarget != null)
        // {
        //     // Antes de passar para o próximo, o alvo atual volta a ser sugestão
        //     currentTarget.SetTargetALock(false);
        //     currentTarget.SetTargetVisual(true);

        //     int currentIndex = _targets.IndexOf(currentTarget);

        //     // Se for o último, cancela o lock (Toggle Off)
        //     if (currentIndex + 1 >= _targets.Count)
        //     {
        //         currentTarget = null;
        //         return false;
        //     }
        //     currentTarget = _targets[currentIndex + 1];
        // }
        // else
        // {
        // }

        // Ativa o lock no novo alvo e esconde a sugestão amarela
        if (currentTarget == null)
        {
            currentTarget = _targets[0];
            currentTarget.SetTargetALock(true);
            currentTarget.SetTargetVisual(false);
        }
        else
        {
            ClearCurrentTarget();
        }

        return currentTarget != null;
    }
    public void ClearCurrentTarget()
    {
        if (currentTarget == null) return;
        currentTarget.SetTargetALock(false);
        currentTarget.SetTargetVisual(true);
        currentTarget = null;
    }
}