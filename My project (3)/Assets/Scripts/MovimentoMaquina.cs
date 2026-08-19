using UnityEngine;

public class MovimentoMaquina : MonoBehaviour
{
    public float velocidadeFrente = 50f;
    public float velocidadeLado = 5f;

    void Update()
    {
        // Pega o scroll do mouse (Frente/Tr�s)
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Pega os bot�es A/D ou Setas (Esquerda/Direita)
        float horizontal = Input.GetAxis("Horizontal");

        // Monta o vetor de movimento
        // X = Esquerda/Direita | Y = 0 | Z = Frente/Tr�s
        Vector3 direcao = new Vector3(horizontal * velocidadeLado, 0, scroll * velocidadeFrente);

        // Aplica o movimento
        transform.Translate(direcao * Time.deltaTime, Space.World);
    }
}