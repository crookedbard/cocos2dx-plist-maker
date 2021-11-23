using System.Text;

namespace Cocos2dxXmlMaker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Csv Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "csv",
                Filter = "csv files (*.csv)|*.csv",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
 <dict>
  <key>frames</key>
  <dict>";
                
                var y = 0;
                var rowNo = 1;
                var rows = File.ReadAllLines(openFileDialog1.FileName);
                foreach (var row in rows)
                {
                    var x = 0;
                    var colNo = 1;
                    var columns = row.Split(';');
                    foreach (var column in columns)
                    {
                        if (string.IsNullOrEmpty(column)) throw new InvalidOperationException("All cells in table should be filled.");

                        var w = Convert.ToInt32(column.Split(',')[0]);
                        var h = Convert.ToInt32(column.Split(',')[1]);
						
                        var item = @"
   <key>tile"+ rowNo+"-"+ colNo + "" + @"</key>
   <dict>
    <key>frame</key>
    <string>{{" + x + "," + y + @"},{" + column + @"}}</string>
    <key>offset</key>
    <string>{0,0}</string>
    <key>rotated</key>
    <false/>
    <key>sourceColorRect</key>
    <string>{{0,0},{" + column + @"}}</string>
    <key>sourceSize</key>
    <string>{" + column + @"}</string>
   </dict>";
                        textBox1.Text += item + Environment.NewLine;

                        x += w;
                        if (colNo == columns.Count())
                        {
                            y += h;
                        }
                        colNo++;
                    }
                    x = 0;
                    rowNo++;
                }

                textBox1.Text += @"
  </dict>

  <key>metadata</key>
  <dict>
   <key>format</key>
   <integer>2</integer>
   <key>size</key>
   <string>{256,512}</string>
   <key>textureFileName</key>
   <string>tilesheet.png</string>
  </dict>
 </dict>
</plist>";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            Stream myStream;
            if ((myStream = saveFileDialog1.OpenFile()) == null) return;
            myStream.Write(Encoding.UTF8.GetBytes(textBox1.Text));
            // Code to write the stream goes here.
            myStream.Close();
        }
    }
}