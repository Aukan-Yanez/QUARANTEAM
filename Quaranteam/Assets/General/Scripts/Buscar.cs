using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buscar : MonoBehaviour
{
    /// <summary>
    /// Obtiene los elementos del tipo 'returnType' al rededor de 'thisGameObject' que estén dentro del radio 'radio' y los retorna como un array.
    /// </summary>
    /// <param name="thisGameObject">Elemento alrededor del cual buscar</param>
    /// <param name="radio">Se retornaran elementos dentro de este radio</param>
    /// <param name="returnType">Tipo de elementos a retornar</param>
    /// <returns></returns>
    public Object[] getElemntsArround(GameObject thisGameObject, float radio, string returnType)
    {
        Vector2 centerPosition2D = new Vector2(thisGameObject.transform.position.x, thisGameObject.transform.position.y);
        
        Object[] returnTypeElements = FindObjectsOfType(System.Type.GetType(returnType));
        List<Object> retorno = new List<Object>();
        foreach (Object element in returnTypeElements)
        {
            Transform hasTransform = GameObject.Find(element.name).GetComponent<Transform>();
            if (hasTransform)
            {
                if (estaDentroDelRadio(centerPosition2D, radio, hasTransform))
                {
                    retorno.Add(element);
                }
            }
        }
        return retorno.ToArray();
    }




    public dynamic getElemntArround(GameObject thisGameObject, string otherGameObjectName, string otherGameObjectType, float radio)
    {
        Vector2 centerPosition2D = new Vector2(thisGameObject.transform.position.x, thisGameObject.transform.position.y);

        Object[] returnTypeElements = FindObjectsOfType(System.Type.GetType(otherGameObjectType));
        List<dynamic> retorno = new List<dynamic>();
        foreach (Object element in returnTypeElements)
        {
            if (element.name == otherGameObjectName)
            {
                Transform hasTransform = GameObject.Find(element.name).GetComponent<Transform>();
                if (hasTransform)
                {
                    if (estaDentroDelRadio(centerPosition2D, radio, hasTransform))
                    {
                        return element;
                    }
                }
            }
        }
        return null;
    }

    public bool isElemntArround(GameObject thisGameObject, string otherGameObjectName, string otherGameObjectType, float radio)
    {
        Vector2 centerPosition2D = new Vector2(thisGameObject.transform.position.x, thisGameObject.transform.position.y);

        Object[] returnTypeElements = FindObjectsOfType(System.Type.GetType(otherGameObjectType));
        foreach (Object element in returnTypeElements)
        {
            if (element.name == otherGameObjectName)
            {
                Transform hasTransform = GameObject.Find(element.name).GetComponent<Transform>();
                if (hasTransform)
                {
                    if (estaDentroDelRadio(centerPosition2D, radio, hasTransform))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    



    private bool estaDentroDelRadio(Vector2 center, float radio, Transform transform)
    {
        Vector2 elementPos = new Vector2(transform.position.x, transform.position.y);
        float distance = (elementPos - center).magnitude;
        if (distance < radio)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Obtiene los elementos que están en contacto directo con 'thisGameObject' y los retorna como un array. Es necesario que 'thisGameObject' posea el componente Rigidbody2D
    /// y que los elementos a buscar posean el componente Collider2D.
    /// </summary>
    /// <param name="thisGameObject">GameObject que esta en contacto directo con los elementos</param>
    /// <param name="returnType">Tipo de los elementos a obtener si son tocados por 'thisgameObject'</param>
    /// <returns></returns>
    public Object[] getTouchedElements(string returnType, GameObject thisGameObject)
    {
        Object[] returnTypeElements = FindObjectsOfType(System.Type.GetType(returnType));
        List<Object> retorno = new List<Object>();
        foreach (Object element in returnTypeElements)
        {
            Collider2D hasCollider = GameObject.Find(element.name).GetComponent<Collider2D>();
            Rigidbody2D hasRigidbody = thisGameObject.GetComponent<Rigidbody2D>();
            if (hasRigidbody && hasCollider)
            {
                if (hasRigidbody.IsTouching(hasCollider))
                {
                    retorno.Add(element);
                }
            }
            else
            {
                Debug.LogWarning("Uno de los elementos buscados por 'Buscar.getTouchedElements()' no posee el componente Rigidbody2D o Collider2D");
            }
        }
        return retorno.ToArray();
    }


}
