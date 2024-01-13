//$TYPE CLASS
//$REFERENCES System.dll; System.Windows.Forms.dll
//$CLASS MyPlugin
//$RUN Start2

using System;
using System.Windows.Forms;

class MyPlugin
{
    private Form frm = null;
    private Label lbl = null;
    public void Start()
    {
        frm = new Form();
        lbl = new Label();
        lbl.Parent = frm;
        lbl.Text = "hello";
        
        frm.ShowDialog();
    }

    public void Start2()
    {
        Console.WriteLine("hello world");
    }
}