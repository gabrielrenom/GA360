import { useState } from 'react';

import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';

// project import
import MainCard from 'components/MainCard';
import IncomeAreaChart from './IncomeAreaChart';
import ActiveLearnersPerMonthAreaChart from './ActiveLearnersPerMonthAreaChart';

// ==============================|| DEFAULT - Active Learners ||============================== //

export default function ActiveLearnersChartCard() {
  const [slot, setSlot] = useState('month');

  return (
    <>
        <Box sx={{ pt: 1, pr: 2 }}>
          <ActiveLearnersPerMonthAreaChart slot={slot} />
        </Box>
    </>
  );
}
