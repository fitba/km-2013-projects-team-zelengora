using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace WindowsServiceSimAndPred
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        public double PearsonSim(int[] item1, int[] item2)
        {
            double _personSim = 0;
            double _averageVotesItem1;
            double _averageVotesItem2;

            double[] _difVotesAverageVotesItem1 = null;
            double[] _difVotesAverageVotesItem2 = null;

            double[] _product_difVotesAverageVotesItem1_difVotesAverageVotesItem2 = null;

            double _numerator; // sum of _product_difVotesAverageVotesItem1_difVotesAverageVotesItem2


            double[] _powerOf_difVotesAverageVotesItem1 = null;
            double[] _powerOf_difVotesAverageVotesItem2 = null;

            double _sumOf_powerOf_difVotesAverageVotesItem1 ;
            double _sumOf_powerOf_difVotesAverageVotesItem2 ;

            double _sqrtOF_sumOf_powerOf_difVotesAverageVotesItem1;
            double _sqrtOF_sumOf_powerOf_difVotesAverageVotesItem2;

            item1[0] = 1;
            item1[1] = 3;
            item1[2] = 5;

            item2[0] = 2;
            item2[1] = 2;
            item2[2] = 4;


            _averageVotesItem1 = item1.Average();
            _averageVotesItem2 = item2.Average();

            _difVotesAverageVotesItem1[0] = item1[0] - _averageVotesItem1;
            _difVotesAverageVotesItem1[1] = item1[1] - _averageVotesItem1;
            _difVotesAverageVotesItem1[2] = item1[2] - _averageVotesItem1;

            _difVotesAverageVotesItem2[0] = item1[0] - _averageVotesItem2;
            _difVotesAverageVotesItem2[1] = item1[1] - _averageVotesItem2;
            _difVotesAverageVotesItem2[2] = item1[2] - _averageVotesItem2;

            _product_difVotesAverageVotesItem1_difVotesAverageVotesItem2[0] = _difVotesAverageVotesItem1[0] * _difVotesAverageVotesItem2[0];
            _product_difVotesAverageVotesItem1_difVotesAverageVotesItem2[1] = _difVotesAverageVotesItem1[1] * _difVotesAverageVotesItem2[1];
            _product_difVotesAverageVotesItem1_difVotesAverageVotesItem2[0] = _difVotesAverageVotesItem1[0] * _difVotesAverageVotesItem2[0];

            _numerator = _product_difVotesAverageVotesItem1_difVotesAverageVotesItem2.Sum();



            _powerOf_difVotesAverageVotesItem1[0] = _difVotesAverageVotesItem1[0] * _difVotesAverageVotesItem1[0];
            _powerOf_difVotesAverageVotesItem1[1] = _difVotesAverageVotesItem1[1] * _difVotesAverageVotesItem1[1];
            _powerOf_difVotesAverageVotesItem1[0] = _difVotesAverageVotesItem1[2] * _difVotesAverageVotesItem1[2];

            _powerOf_difVotesAverageVotesItem2[0] = _difVotesAverageVotesItem2[0] * _difVotesAverageVotesItem2[0];
            _powerOf_difVotesAverageVotesItem2[1] = _difVotesAverageVotesItem2[1] * _difVotesAverageVotesItem2[1];
            _powerOf_difVotesAverageVotesItem2[0] = _difVotesAverageVotesItem2[2] * _difVotesAverageVotesItem2[2];

            _sumOf_powerOf_difVotesAverageVotesItem1 = _powerOf_difVotesAverageVotesItem1.Sum();
            _sumOf_powerOf_difVotesAverageVotesItem2 = _powerOf_difVotesAverageVotesItem2.Sum();

            _sqrtOF_sumOf_powerOf_difVotesAverageVotesItem1 = Math.Sqrt(_sumOf_powerOf_difVotesAverageVotesItem1);
            _sqrtOF_sumOf_powerOf_difVotesAverageVotesItem2 = Math.Sqrt(_sumOf_powerOf_difVotesAverageVotesItem2);

            double _denominator = _sqrtOF_sumOf_powerOf_difVotesAverageVotesItem1 * _sqrtOF_sumOf_powerOf_difVotesAverageVotesItem2;

            _personSim = _numerator / _denominator;





            Console.Write(_personSim);
            Console.Read();


            return _personSim;
         
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {
        }
    }
}
