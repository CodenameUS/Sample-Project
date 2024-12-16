using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                PortionItem : ���� ������
                
                Use() : IUsableItem �������̽� ���
                    - (�ӽ�) ���� �ϳ� ����, �������� ��ȯ
 */

public class PortionItem : CountableItem, IUsableItem
{
    public PortionItem(PortionItemData data, int amount = 1) : base(data, amount) { }

    public bool Use()
    {
        Amount--;

        return true;
    }
}
