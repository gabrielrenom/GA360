import React, { useContext, useEffect, useState } from 'react';
import { useTheme } from '@mui/material/styles';
import ReactApexChart, { Props as ChartProps } from 'react-apexcharts';
import { ThemeMode } from 'config';
import useConfig from 'hooks/useConfig';
import { GetActiveLearnersPerMonth, ActiveLearnersPerMonth } from 'api/customer';
import DuendeContext from 'contexts/DuendeContext';

const areaChartOptions = {
  chart: {
    height: 450,
    type: 'area',
    toolbar: {
      show: false
    }
  },
  dataLabels: {
    enabled: false
  },
  stroke: {
    curve: 'smooth',
    width: 2
  },
  grid: {
    strokeDashArray: 0
  }
};

interface Props {
  slot: string;
}

export default function ActiveLearnersPerMonthAreaChart({ slot }: Props) {
  const theme = useTheme();
  const { mode } = useConfig();
  const { user } = useContext(DuendeContext);

  const { primary, secondary } = theme.palette.text;
  const line = theme.palette.divider;

  const [options, setOptions] = useState<ChartProps>(areaChartOptions);
  const [series, setSeries] = useState([{ name: 'Active Learners', data: [] }]);

  useEffect(() => {
    setOptions((prevState) => ({
      ...prevState,
      colors: [theme.palette.primary.main, theme.palette.primary[700]],
      xaxis: {
        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        labels: {
          style: {
            colors: [secondary, secondary, secondary, secondary, secondary, secondary, secondary, secondary, secondary, secondary, secondary, secondary]
          }
        },
        axisBorder: {
          show: true,
          color: line
        },
        tickAmount: 11
      },
      yaxis: {
        labels: {
          style: {
            colors: [secondary]
          }
        }
      },
      grid: {
        borderColor: line
      },
      theme: {
        mode: mode === ThemeMode.DARK ? 'dark' : 'light'
      }
    }));
  }, [mode, primary, secondary, line, theme, slot]);

  useEffect(() => {
    async function fetchData() {
      try {
        
        const data = await GetActiveLearnersPerMonth(user.trainingCentreId);
        const monthlyData = new Array(12).fill(0);

        data.forEach((item) => {
          const monthIndex = item.month - 1; // Convert month to zero-based index
          monthlyData[monthIndex] += item.learnersCount;
        });

        setSeries([{ name: 'Active Learners', data: monthlyData }]);
      } catch (error) {
        console.error('Failed to fetch active learners per month:', error);
      }
    }

    fetchData();
  }, [user.trainingCentreId]);

  return <ReactApexChart options={options} series={series} type="area" height={450} />;
}
