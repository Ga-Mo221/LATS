using UnityEngine;

public abstract partial class EnemyBase
{
    #region Damage Text
    /// <summary>
    /// Tạo mới DamageText instance
    /// </summary>
    private void createDamageText(int index)
    {
        GameObject dmg = Instantiate(GameModule.Instance._damageTextPrefab, _startDamageTextPos.position, Quaternion.identity, _canvasDamageText);
        dmg.SetActive(false);
        _damageTexts[index] = dmg;
    }

    /// <summary>
    /// Hiển thị DamageText khi enemy nhận damage
    /// </summary>
    protected void displayDamager(float damage, bool magic)
    {
        for (int i = 0; i < _damageTexts.Length; i++)
        {
            if (_damageTexts[i] == null)
                createDamageText(i);

            DamageText script = _damageTexts[i].GetComponent<DamageText>();
            if (!script._start)
            {
                _damageTexts[i].SetActive(true);
                script.setDamage(damage, magic, _startDamageTextPos.position);
                script._start = true;
                return;
            }
        }
    }
    #endregion

}
