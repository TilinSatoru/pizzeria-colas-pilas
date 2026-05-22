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
        private Queue<string> colaPedidos = new Queue<string>();
        private Stack<string> pilaBitacora = new Stack<string>();

        public MainForm()
        {
            InitializeComponent();
            ActualizarUI();
        }

        private void BtnNuevoPedido_Click(object sender, EventArgs e)
        {
            // Vaciado para el commit 1
        }

        private void BtnEntregar_Click(object sender, EventArgs e)
        {
            // Vaciado para el commit 1
        }

        private void BtnDeshacer_Click(object sender, EventArgs e)
        {
            // Vaciado para el commit 1
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