using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using GTA.Math;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

public class debug : Script
{
    private int compact_spawned = 0;
    private int x = 0;
    private float distance = 200.0f;
    private int all_coords;

    private Blip[] marker = new Blip[103];
    private Vector3[] coords = new Vector3[103];
    private GTA.Vehicle[] cars_hashes = new GTA.Vehicle[5];
    private List<int> models = new List<int>() { 15214558, 1429622905, 1644055914, 409049982, 1118611807, 931280609, 1549126457 };

    public debug()
    {
        KeyDown += onkeydown;

        coords[0] = new Vector3(-3260.979f, 3524.306f, 1.150f);

        all_coords = 5;
    }

    void onkeydown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Z && compact_spawned == 0)
        {
            int id = 686;
            for (int i = 0; i <= all_coords; i++)
            {
                marker[i] = GTA.Native.Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_COORD, coords[i].X, coords[i].Y, coords[i].Z);
                GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, marker[i], id);
                id++;
            }
            compact_spawned = 1;
        }
        else
        {
            if (e.KeyCode == Keys.Z && compact_spawned == 1)
            {
                for (int i = 0; i <= all_coords; i++)
                {
                    marker[i].Remove();
                }
                compact_spawned = 0;
            }
        }
    }
}