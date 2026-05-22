﻿/*
 * Pizzería Campus Express - Gestión de pedidos con Queue y Stack
 * Compatible con SharpDevelop 4.4 / .NET Framework 2.0+
 */

using System;
using System.Collections; // Mantenemos la compatibilidad heredada
using System.Windows.Forms;

namespace laboratoriPizzeriaExpress
{
    public partial class MainForm : Form
    {
        private Queue colaPedidos = new Queue();
        private Stack pilaBitacora = new Stack();

        public MainForm()
        {
            InitializeComponent();
            ActualizarUI();
        }

        private void BtnNuevoPedido_Click(object sender, EventArgs e)
        {
            string cliente = txtCliente.Text.Trim();
            
            // VALIDACIÓN: Evitar registrar textos vacíos
            if (string.IsNullOrEmpty(cliente))
            {
                lblEstado.Text = "⚠️ Error: Debes ingresar el nombre del cliente.";
                return;
            }

            colaPedidos.Enqueue(cliente);
            pilaBitacora.Push(string.Format("PEDIDO: {0}", cliente));
            
            txtCliente.Clear();
            lblEstado.Text = string.Format("✅ Pedido registrado para {0}", cliente);
            ActualizarUI();
        }

        private void BtnEntregar_Click(object sender, EventArgs e)
        {
            // VALIDACIÓN: Evitar excepciones si la cola está vacía
            if (colaPedidos.Count == 0)
            {
                lblEstado.Text = "❌ No hay pedidos pendientes.";
                return;
            }

            string cliente = (string)colaPedidos.Dequeue();
            pilaBitacora.Push(string.Format("ENTREGADO: {0}", cliente));
            lblEstado.Text = string.Format("🍕 Pedido entregado a {0}", cliente);
            ActualizarUI();
        }

        private void BtnDeshacer_Click(object sender, EventArgs e)
        {
            // Se mantiene vacío para el Commit 3
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            colaPedidos.Clear();
            pilaBitacora.Clear();
            lblEstado.Text = "🧹 Sistema reiniciado.";
            ActualizarUI();
        }

        private void ActualizarUI()
        {
            lstPedidos.Items.Clear();
            lstBitacora.Items.Clear();

            foreach (string p in colaPedidos) lstPedidos.Items.Add(p);
            if (colaPedidos.Count == 0) lstPedidos.Items.Add("(Sin pedidos pendientes)");

            foreach (string accion in pilaBitacora) lstBitacora.Items.Add(accion);
            if (pilaBitacora.Count == 0) lstBitacora.Items.Add("(Sin acciones registradas)");

            lblContador.Text = string.Format("Pedidos: {0} | Bitácora: {1}", colaPedidos.Count, pilaBitacora.Count);
        }
    }
}