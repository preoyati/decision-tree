using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace c4._5
{
    public partial class main : Form
    {
        
        int attr=0;int attr_val_no=0;string[,] attr_val=new string[500,500];
   
    
        public main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string problem = textBox1.Text;
            string tex = File.ReadAllText("data/data.txt");
            string[] div = tex.Split('\n');
            int div_len = div.Length;
            string[,] prblem = new string[div_len, 5];
            string[] dat = new string[5];
            int[] store = new int[100];
            int[] store_value_num = new int[100];
            string[,] store_value = new string[100, 100];
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            for (i = 0; i < div_len; i++)
            {

                string[] prb = div[i].ToString().Split(' ');


                for (j = 0; j < 5; j++)
                {
                    prblem[i, j] = prb[j].ToString();


                }

            }
            for (i = 0; i < div_len; i++)
            {

                if (prblem[i, 0] == problem)
                {
                    for (j = 0; j < 5; j++)
                    {
                        dat[j] = prblem[i, j];


                    }
                    break;

                }

            }
            string path = "" + dat[0] + ".txt";
            int class_col = int.Parse(dat[2]);
            int attribute = int.Parse(dat[3]);
            int numeric = int.Parse(dat[4]);
            string data = File.ReadAllText(path);
            char[] s = { '\n', '\0' };
            string[] training_set = data.Split(s);
            int train_set_len = training_set.Length;
            int fold_value = train_set_len / int.Parse(dat[1]);
            int input = fold_value * (int.Parse(dat[1]) - 1);
            string[,] table = new string[train_set_len, attribute + numeric + class_col];
            int[] attribute_value_no = new int[attribute + numeric];
            string[] clas = new string[input];
            int[] attribute_selection = new int[1000];
            string[,] attribute_value = new string[attribute + numeric, input];
            float[] gini_reduction = new float[attribute + numeric];
            Console.WriteLine(train_set_len.ToString());

            /******************************take training into table*********************/
            for (i = 0; i < train_set_len; i++)
            {

                string[] uni = training_set[i].ToString().Split(' ');


                for (j = 0; j < attribute + numeric + class_col; j++)
                {
                    
                   table[i, j] = uni[j].ToString();
                   
                  
               }

             }
            /*********************class after joining the columns********************/
            string[] clas_cl = new string[train_set_len];
            for (j = 0; j < train_set_len; j++)
            {
                for (i = 0; i < class_col; i++)
                {
                    clas_cl[j] = "" + clas_cl[j] + "" + table[j, attribute + numeric + i] + "";
                }


            }


            /************************how many class here**********************/

            k = 0;
            clas[0] = clas_cl[0];

            for (i = 1; i < train_set_len - 1; i++)
            {
                bool test = true;
                for (l = 0; l <= k; l++)
                {
                    if (clas_cl[i] == clas[l])
                    {
                        test = false;
                        break;

                    }

                }

                if (test != false)
                {
                    k++;
                    clas[k] = clas_cl[i];


                }
               }
            int class_no = k + 1;//number of classes here.
            int[] attr_on_select=new int[attribute+numeric];
            for (i = 0; i < attribute + numeric; i++)
            {
                attr_on_select[i] = i;
            }
                /********************now number of classes****************/
            TreeNode tn = new TreeNode();
               
            tree(table, class_no,clas_cl, clas, class_col, train_set_len, attribute, numeric, 0, attr_on_select,0,tn,store,store_value_num,store_value);
 
        }

        void attribute_selection_func(string[,] table, int class_no, string[] clas_cl, string[] clas, int input, int attribute, int numeric, int class_col,int at_num,int[] att_on_select)
        {
           
            int i,j,k,l;
          
            double[] info_T_X = new double[200];
            double[] info_T_X_1 = new double[200];
            double[] gain = new double[200];
            double[] split_info = new double[200];
            double[] gain_ratio=new double[200];
            int[] attribute_selection = new int[200];
            Console.WriteLine("here the results");
         
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("classes here");
        for (l = 0; l < class_no; l++)
        {
            Console.WriteLine(clas[l]);
        }
        Console.WriteLine("finish");
         

            /*******************calculating infox*************/
            string[,] attribute_value = new string[attribute + numeric, 200]; //var
            double[, ,] frequency = new double[attribute + numeric, 200, 200];//var
            double[,] Tx = new double[200, 200];
            double temp1 = 0.0;
            double temp2 = 0.0;
            int[] attribute_value_no = new int[attribute + numeric];

            for (j = 0; j < attribute;j++ )
            {
                float[] count = new float[200];
                double info_T = 0.0;
                int inp = 0;
                int fract = 0;
                for (i = 0; i < input; i++)
                {
               
                    if (table[i, j].Equals("?"))
                    {
                     //  inp--;
                        fract++;
                        
                    }
                    else
                    {
                        for (l = 0; l < class_no; l++)
                        {
                            if (table[i, attribute + numeric] == clas[l])
                            {

                                count[l] = count[l] + 1;


                            }
                        }  
                    }
                }
                for (l = 0; l < class_no; l++)
                {
                    inp += int.Parse(count[l].ToString());
                }
                Console.WriteLine("input:"+inp.ToString()+"");

                for (l = 0; l < class_no; l++)
                {
                 //   Console.WriteLine("class number:" + count[l].ToString() + "");
                    info_T = info_T + (-(count[l] / inp) * Math.Log((count[l] / inp), 2.0));
                }
                Console.WriteLine("info class:");
                Console.WriteLine(info_T.ToString());

               if (att_on_select[j] != -5)
                {
                    k = 0;
                    attribute_value[j, 0] = table[0, j];
                    for (i = 1; i < input; i++)
                    {
                        if (table[i, j].Equals("?"))
                        {
                        }
                        else
                        {
                            bool test = true;
                            for (l = 0; l <= k; l++)
                            {
                                if (table[i, j] == attribute_value[j, l])
                                {
                                    test = false;
                                    break;



                                }

                            }

                            if (test != false)
                            {
                                k++;
                                attribute_value[j, k] = table[i, j];
                            }

                        }
                    }
                    attribute_value_no[j] = k + 1;
              
                    for (l = 0; l < class_no; l++)
                    {

                        for (int m = 0; m < attribute_value_no[j]; m++)
                        {
                            for (i = 0; i < inp+fract; i++)
                            {
                                if (table[i, j] == attribute_value[j, m])
                                {

                                    if (clas_cl[i] == clas[l])
                                    {

                                        frequency[j, l, m] = frequency[j, l, m] + 1;


                                    }

                                }

                            }


                        }
                    }

                    

                    for (int m = 0; m < attribute_value_no[j]; m++)
                    {
                        for (i = 0; i < inp+fract; i++)
                        {
                            if (table[i, j] == attribute_value[j, m])
                            {



                                Tx[j, m] = Tx[j, m] + 1;

                            }

                        }


                    }
                    info_T_X[j] = 0.0;
                    info_T_X_1[j] = 0.0;
                    info_T_X[j] = 0.0;
                    for (int m = 0; m < attribute_value_no[j]; m++)
                    {
                        for (l = 0; l < class_no; l++)
                        {
                            temp1 = frequency[j, l, m];
                            temp2 = Tx[j, m];
                        
                            
                            if (temp1 != 0)
                                info_T_X_1[j] = info_T_X_1[j] + (-(temp1 / temp2) * Math.Log((temp1 / temp2), 2.0));

                        }
                        info_T_X[j] = info_T_X[j] + ((temp2 / inp) * info_T_X_1[j]);
                        split_info[j] = split_info[j] + (-((temp2 / (inp+fract)) * Math.Log((temp2 / (inp+fract)), 2.0)));
                        info_T_X_1[j] = 0.0;
                    }
                   // 
                    if (fract != 0)
                    {
                        double fr = double.Parse(fract.ToString());
                        split_info[j] = split_info[j] + (-((fr / (inp + fract)) * Math.Log((fr / (inp + fract)), 2.0)));
                        
                    }
               
                    double fraction = double.Parse(inp.ToString()) / double.Parse((inp + fract).ToString());
                    gain[j] = fraction*(info_T - info_T_X[j]);
                    
                    gain_ratio[j] = gain[j] / split_info[j];
                    attribute_selection[j] = j;
                }
                else
                {
                    gain_ratio[j] = -1;
                    attribute_selection[j] = j;
                }
              
            }

          


            /*******************continuous value*********************/
              
            if (numeric > 0)
            {
                string[,] table2 = new string[input, attribute + numeric + class_col];
                for (i = 0; i < input; i++)
                {
                    for (int all = 0; all < attribute + numeric + class_col; all++)
                    {

                        table2[i, all] = table[i, all];

                    }
                }
                double[,] threshhold = new double[200, 200];
                double max;
                double[] gain_thresh = new double[200];
                double[] gain_ratio_thresh = new double[200];
                for (j = 0; j < numeric; j++)
                {
                    float[] count = new float[200];
                    double info_T = 0.0;
                    int inp=0;
                    int fract = 0;
                    for (i = 0; i < input; i++)
                    {
                        if (table2[i, j+attribute].Equals("?"))
                        {
                            //  inp--;
                            fract++;

                        }
                        else
                        {
                            for (l = 0; l < class_no; l++)
                            {
                                if (table2[i, attribute + numeric] == clas[l])
                                {

                                    count[l] = count[l] + 1;


                                }
                            }
                        }
                    }

                    for (l = 0; l < class_no; l++)
                    {
                        inp += int.Parse(count[l].ToString());
                    }
                  //  Console.WriteLine("input:" + inp.ToString() + "");

                    for (l = 0; l < class_no; l++)
                    {
                        info_T = info_T + (-(count[l] / inp) * Math.Log((count[l] / inp), 2.0));
                    }
                 

                   if (att_on_select[j+attribute]!= -5)
                    {
                        Console.WriteLine("next");
                        int th_num = 0;
                        string boundary;
                        for (i = 0; i < inp+fract; i++)
                        {

                            for (k = 0; k < i; k++)
                            {


                                if (float.Parse(table2[i, j + attribute]) < float.Parse(table2[k, j + attribute]))
                                {
                                    for (int ch = 0; ch < attribute + numeric + class_col; ch++)
                                    {
                                        string temporary = table2[k, ch];
                                        table2[k, ch] = table2[i, ch];
                                        table2[i, ch] = temporary;
                                    }

                                }

                            }
                        }
                        boundary = table2[0, attribute + numeric];
                        for (i = 1; i < inp; i++)
                        {
                            if (table2[i, attribute + numeric] != boundary)
                            {
                                threshhold[j, th_num] = (float.Parse(table2[i - 1, attribute + j]) + float.Parse(table2[i, attribute + j])) / 2;
                                boundary = table2[i, attribute + numeric];
                      
                                th_num++;


                            }
                        }
                        double[, ,] frequency_th = new double[input, input, input];

                        //****************************************************************************/

                        for (int s = 0; s < th_num; s++)
                        {
                           
                            for (l = 0; l < class_no; l++)
                            {



                                for (i = 0; i < inp; i++)
                                {
                                    if (double.Parse(table2[i, attribute + j]) <= threshhold[j, s])
                                    {

                                        if (table2[i, attribute + numeric] == clas[l])
                                        {



                                            frequency_th[s, l, 0] = frequency_th[s, l, 0] + 1;


                                        }

                                    }
                                    else
                                    {

                                        if (table2[i, attribute + numeric] == clas[l])
                                        {


                                            frequency_th[s, l, 1] = frequency_th[s, l, 1] + 1;
                                           


                                        }
                                    }


                                }


                            }




                            Tx[s, 0] = 0;
                            Tx[s, 1] = 0;
                            for (i = 0; i < inp; i++)
                            {
                                if (double.Parse(table2[i, attribute + j]) <= threshhold[j, s])
                                {



                                    Tx[s, 0] = Tx[s, 0] + 1;


                                }
                                else
                                {
                                    Tx[s, 1] = Tx[s, 1] + 1;

                                }

                            }




                            info_T_X[s] = 0.0;
                            info_T_X_1[s] = 0.0;
                            info_T_X[s] = 0.0;
                            for (int m = 0; m < 2; m++)
                            {
                                for (l = 0; l < class_no; l++)
                                {
                                    temp1 = frequency_th[s, l, m];
                                    temp2 = Tx[s, m];
                                  
                                    if (temp1 != 0)
                                        info_T_X_1[s] = info_T_X_1[s] + (-(temp1 / temp2) * Math.Log((temp1 / temp2), 2.0));

                                }
                               
                                info_T_X[s] = info_T_X[s] + ((temp2 / inp) * info_T_X_1[s]);
                                split_info[s] = split_info[s] + (-((temp2 / inp) * Math.Log((temp2 / inp), 2.0)));
                                info_T_X_1[s] = 0.0;
                            }

                            gain_thresh[s] = info_T - info_T_X[s];
                            gain_ratio_thresh[s] = gain_thresh[s] / split_info[s];
                           
                        }
                        max = gain_ratio_thresh[0];
                        int thhh = 0;
                        for (i = 0; i < th_num; i++)
                        {

                            if (gain_ratio_thresh[i] > max)
                            {
                                max = gain_ratio_thresh[i];
                                thhh = i;

                            }
                        }

                        gain_ratio[attribute + j] = max;
                        attribute_selection[attribute + j] = attribute + j;
                   
                        attribute_value[j + attribute, 0] = "<=" + threshhold[j, thhh].ToString() + "";
                        attribute_value[j + attribute, 1] = ">" + threshhold[j, thhh].ToString() + "";
                        attribute_value_no[j + attribute] = 2;
                    }
                    else
                    {
                        gain_ratio[attribute + j] = -1;
                        attribute_selection[attribute + j] = attribute + j;
                    }
                }
              
/**************************************************************************************/
                    }

         
            /*************************************sorting according to gain ratio*****************************/
            Console.WriteLine("iiiiiiiiiiiiiiiiiiiiiiii");
           
            for (i = 0; i < attribute + numeric; i++)
            {
                Console.WriteLine(gain_ratio[i]); 
                for (j = 0; j < i; j++)
                {
                    if (gain_ratio[i] > gain_ratio[j])
                    {
                        double temp = gain_ratio[j];
                        gain_ratio[j] = gain_ratio[i];
                        gain_ratio[i] = temp;
                        int tmp = attribute_selection[j];
                        attribute_selection[j] = attribute_selection[i];
                        attribute_selection[i] = tmp;

                    }
                }

            }
           
            attr = attribute_selection[0];
            attr_val_no = attribute_value_no[attribute_selection[0]];
            attr_val = attribute_value;
            
           
          
              /*********************************end of call*******************************************************/   
        }

        void tree(string[,] tab, int clas_no,string[] clas_cl, string[] clas,int cl_col,int inp,int discrete,int numer,int at,int[] attr_on_sel,int update,TreeNode tre,int[] store,int[] store_value_num,string[,]store_value)
        {
           TreeNode tn = new TreeNode();
          
            string[,] subset = new string[inp, discrete+numer+ cl_col];
            string result = "";
            string[,] nodes = new string[100, 100];
            treeView1.Visible = true;
            int c = 0;
            int i = 0;
            int j = 0;
            int l = 0;
            bool xy = false;
            if (clas_no == 1)
            {

                
                result = clas[0];
                tn.Text = result;
                tre.Nodes.Add(tn);
                try
                {
                    File.AppendAllText(@"result/" + textBox1.Text + ".txt", "" + store[update - 1] + "(" + store_value[store[update - 1], at] + ")" + result + "\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(update.ToString());
                }
                return;
            }
           
            attribute_selection_func(tab, clas_no, clas_cl, clas, inp, discrete, numer, cl_col, at, attr_on_sel);
            store[update] = attr;
            store_value_num[update] = attr_val_no;
            for (i = 0; i < attr_val_no; i++)
            {
                store_value[store[update], i] = attr_val[attr, i];
            }
           
            
              int re = update+1;

              if (update == 0)
              {
                  tn.Text = store[update].ToString();
                  treeView1.Nodes.Add(tn);
               }

              else
              {
                  tn.Text = store[update].ToString();
                  tre.Nodes.Add(tn);
                  if (store[update-1] != store[update])
                  {
                    try
                      {
                          File.AppendAllText(@"result/" + textBox1.Text + ".txt", "" + store[update-1] + "(" + store_value[store[update - 1], at] + "),");
                      }
                      catch (Exception ex)
                      {
                          Console.WriteLine(update.ToString());
                      }
                  }
              }
               attr_on_sel[store[update]] = -5;
                for (j = 0; j < store_value_num[update]; j++)
                {
                    
                    int n = 0;
                    for (l = 0; l < inp; l++)
                    {
                        if (store[update] < discrete)
                        {
                            if (store_value[store[update], j] == tab[l, store[update]])
                            {
                                for (int m = 0; m < discrete+numer + cl_col; m++)
                                {
                                    subset[n, m] = tab[l, m];

                                }
                                n++;
                            }
                        }
                        else
                        {
                           
                            if (j == 0)
                            {

                                if (float.Parse(tab[l, store[update]]) <= float.Parse((store_value[store[update], j]).Split('=')[1]))
                                {
                                    for (int m = 0; m < discrete + numer + cl_col; m++)
                                    {
                                        subset[n, m] = tab[l, m];
                                        

                                    }
                                    
                                    n++;
                                }
                            }
                            else  if (j == 1)
                            {
                                if (float.Parse(tab[l, store[update]]) > float.Parse((store_value[store[update], j]).Split('>')[1]))
                                {
                                    for (int m = 0; m < discrete + numer + cl_col; m++)
                                    {
                                        subset[n, m] = tab[l, m];
                                        

                                    }
                                    
                                    n++;
                                }
                            }

                        }

                    }
                    
                    for (int inc = 0; inc < n; inc++)
                    {
                        for (i = 0; i < cl_col; i++)
                        {
                            clas_cl[inc] = "" + clas_cl[inc] + "" + subset[inc, discrete+numer + i] + "";
                        }
                       

                    }
                    
                    clas[0] = subset[0,discrete+numer];
                    int len = 0;
                   
                    for (i = 1; i < n; i++)
                    {
                        bool test = true;
                        for (c = 0; c <= len; c++)
                        {
                            if (subset[i, discrete + numer] == clas[c])
                            {
                                test = false;
                                break;
                            }

                        }
                       if (len == clas_no - 1)
                        {
                            break;
                        }
                        
                        if (test != false)
                        {
                            len++;
                            clas[len] = subset[i, discrete + numer];

                        }


                    }

                  
                     
                          if (n != 0)
                          tree(subset, len + 1, clas_cl, clas, cl_col, n, discrete, numer, j, attr_on_sel, re, tn, store, store_value_num, store_value);
                         
                }

                

        }
       
    }
}
