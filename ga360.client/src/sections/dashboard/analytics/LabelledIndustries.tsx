import React, { useContext, useEffect, useState } from 'react';
// material-ui
import Divider from '@mui/material/Divider';
import Grid from '@mui/material/Grid';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemAvatar from '@mui/material/ListItemAvatar';
import ListItemText from '@mui/material/ListItemText';
import Typography from '@mui/material/Typography';

// project import
import Avatar from 'components/@extended/Avatar';
import LinearWithLabel from 'components/@extended/progress/LinearWithLabel';
import MainCard from 'components/MainCard';

// assets
import Target from 'assets/images/analytics/target.svg';

// service
import { getIndustriesStats, getIndustriesStatsByTrainingCentreId } from 'api/dashboardService';
import DuendeContext from 'contexts/DuendeContext';

export default function LabelledIndustries() {
  const [industries, setIndustries] = useState([]);
  const { user, isLoggedIn } = useContext(DuendeContext);

  useEffect(() => {
    async function fetchData() {
      try {
        if (user.role == "Super Admin")
        {
          const data = await getIndustriesStats();
          setIndustries(data);
        }
        else
        {
          const data = await getIndustriesStatsByTrainingCentreId(user.trainingCentreId);
          setIndustries(data);
        }
      } catch (error) {
        console.error('Error fetching industry stats:', error);
      }
    }

    fetchData();
  }, []);

  const getRandomColor = () => {
    const colors = ["error", "warning", "primary", "success"];
    const randomIndex = Math.floor(Math.random() * colors.length);
    return colors[randomIndex];
};

  return (
    <Grid item xs={12}>
      <MainCard sx={{ width: '100%' }}>
        <Grid container spacing={1.25}>
          {industries.map((industry) => (
            <Grid item xs={12} key={industry.industry}>
              <Typography>{industry.industry}</Typography>
                            <LinearWithLabel 
                            value={industry.percentage} 
                            // @ts-ignore
                            color={getRandomColor()} />
            </Grid>
          ))}
          <Grid item xs={12}>
            <Divider />
          </Grid>
          <Grid item xs={12}>
            <List sx={{ pb: 0 }}>
              <ListItem sx={{ p: 0 }}>
                <ListItemAvatar>
                  <Avatar sx={{ background: 'transparent' }}>
                    <img alt="target" src={Target} />
                  </Avatar>
                </ListItemAvatar>
                <ListItemText
                  primary="Top industries"
                  secondary="Top industries covered by GA360."
                />
              </ListItem>
            </List>
          </Grid>
        </Grid>
      </MainCard>
    </Grid>
  );
}
