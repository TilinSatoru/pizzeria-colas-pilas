﻿/*
 * Pizzería Campus Express - Gestión de pedidos con Queue y Stack
 * Compatible con SharpDevelop 4.4 / .NET Framework 2.0+
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace laboratoriPizzeriaExpress
{
    public partial class MainForm : Form
    {
        // Colecciones principales: FIFO para pedidos normales, FIFO para Premium, LIFO para bitácora
        private Queue<string> colaPedidos = new Queue<string>();
        private Queue<string> colaPedidosPremium = new Queue<string>(); // NUEVA COLA PREMIUM
        private Stack<string> pilaBitacora = new Stack<string>();

        public MainForm()
        {
            InitializeComponent();
            ActualizarUI();
        }

        // PASO 1: Nuevo pedido Normal
        private void BtnNuevoPedido_Click(object sender, EventArgs e)
        {
            string cliente = txtCliente.Text.Trim();

            if (string.IsNullOrEmpty(cliente))
            {
                lblEstado.Text = "Error chamo: Debes ingresar el nombre del cliente.";
                return;
            }

            colaPedidos.Enqueue(cliente);
            pilaBitacora.Push(string.Format("PEDIDO: {0}", cliente));

            txtCliente.Clear();
            lblEstado.Text = string.Format("Listo, pedido registrado para {0}", cliente);
            ActualizarUI();
        }

        // PASO 2: Entregar pedido (Da prioridad a la cola Premium)
        private void BtnEntregar_Click(object sender, EventArgs e)
        {
            // Primero verificamos si hay pedidos VIP/Premium
            if (colaPedidosPremium.Count > 0)
            {
                string clientePremium = colaPedidosPremium.Dequeue();
                pilaBitacora.Push(string.Format("ENTREGADO PREMIUM: {0}", clientePremium));
                lblEstado.Text = string.Format("Pedido PREMIUM entregado a {0} ⭐", clientePremium);
                ActualizarUI();
                return; 
            }

            // Si no hay Premium, atendemos la cola normal
            if (colaPedidos.Count == 0)
            {
                lblEstado.Text = "No hay pedidos pendientes pana.";
                return;
            }

            string cliente = colaPedidos.Dequeue();
            pilaBitacora.Push(string.Format("ENTREGADO: {0}", cliente));
            lblEstado.Text = string.Format("Pedido entregado a {0}", cliente);
            ActualizarUI();
        }

        // PASO 3: Deshacer última acción 
        private void BtnDeshacer_Click(object sender, EventArgs e)
        {
            if (pilaBitacora.Count == 0)
            {
                lblEstado.Text = "📭 No hay acciones para deshacer.";
                return;
            }

            string ultimaAccion = pilaBitacora.Pop();

            if (ultimaAccion.StartsWith("PEDIDO PREMIUM:"))
            {
                string nombre = ultimaAccion.Replace("PEDIDO PREMIUM: ", "");
                ReconstruirCola(colaPedidosPremium, nombre);
                lblEstado.Text = string.Format("↩️ Se deshizo el pedido premium de {0}", nombre);
            }
            else if (ultimaAccion.StartsWith("PEDIDO:"))
            {
                string nombre = ultimaAccion.Replace("PEDIDO: ", "");
                ReconstruirCola(colaPedidos, nombre);
                lblEstado.Text = string.Format("↩️ Se deshizo el pedido de {0}", nombre);
            }
            else if (ultimaAccion.StartsWith("ENTREGADO PREMIUM:"))
            {
                string nombre = ultimaAccion.Replace("ENTREGADO PREMIUM: ", "");
                colaPedidosPremium.Enqueue(nombre);
                lblEstado.Text = string.Format("↩️ Se deshizo la entrega premium a {0}", nombre);
            }
            else if (ultimaAccion.StartsWith("ENTREGADO:"))
            {
                string nombre = ultimaAccion.Replace("ENTREGADO: ", "");
                colaPedidos.Enqueue(nombre);
                lblEstado.Text = string.Format("↩️ Se deshizo la entrega a {0}", nombre);
            }
            else
            {
                lblEstado.Text = "⚠️ Acción desconocida en bitácora.";
            }

            ActualizarUI();
        }

        // Método auxiliar para reconstruir colas al deshacer
        private void ReconstruirCola(Queue<string> cola, string nombreAEliminar)
        {
            string[] temporal = cola.ToArray();
            cola.Clear();

            foreach (string p in temporal)
            {
                if (p != nombreAEliminar)
                    cola.Enqueue(p);
            }
        }

        // PASO 4: Limpiar todo
        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            colaPedidos.Clear();
            colaPedidosPremium.Clear();
            pilaBitacora.Clear();
            lblEstado.Text = "🧹 Sistema reiniciado.";
            ActualizarUI();
        }

        // Sincronizar la interfaz con el estado actual
        private void ActualizarUI()
        {
            lstPedidos.Items.Clear();
            lstBitacora.Items.Clear();

            // Mostrar primero los premium en la lista visual
            foreach (string p in colaPedidosPremium)
                lstPedidos.Items.Add("[VIP] " + p);

            // Mostrar luego los normales
            foreach (string p in colaPedidos)
                lstPedidos.Items.Add(p);

            if (colaPedidos.Count == 0 && colaPedidosPremium.Count == 0)
                lstPedidos.Items.Add("(Sin pedidos pendientes)");

            // Mostrar bitácora
            foreach (string accion in pilaBitacora)
                lstBitacora.Items.Add(accion);

            if (pilaBitacora.Count == 0)
                lstBitacora.Items.Add("(Sin acciones registradas)");

            int totalPedidos = colaPedidos.Count + colaPedidosPremium.Count;
            lblContador.Text = string.Format("Pedidos totales: {0} | Bitácora: {1}", totalPedidos, pilaBitacora.Count);
        }

        // PASO 1.5: Este es el método que tu diseño está llamando al presionar btnPremium
        void Button1Click(object sender, EventArgs e)
        {
            string cliente = txtCliente.Text.Trim();

            if (string.IsNullOrEmpty(cliente))
            {
                lblEstado.Text = "Error chamo: Debes ingresar el nombre del cliente premium.";
                return;
            }

            // Se agrega a la cola con prioridad
            colaPedidosPremium.Enqueue(cliente);
            pilaBitacora.Push(string.Format("PEDIDO PREMIUM: {0}", cliente));

            txtCliente.Clear();
            lblEstado.Text = string.Format("Listo, pedido PREMIUM registrado para {0} ⭐", cliente);
            ActualizarUI();
        }

        //esto si lo hice con ia profe porque no sabia como resolver el error de los Eventos vacíos que generó Visual Studio y no estamos usando pero deben quedarse para que no de error
        void LstBitacoraSelectedIndexChanged(object sender, EventArgs e) { }
        void MainFormLoad(object sender, EventArgs e) { }
    }
}