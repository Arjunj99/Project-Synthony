using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface powerUpInterface
{
    IEnumerator timeToDeath();
    void powerUp();
    void setTime(int newTime);
}
