import React, { useEffect, useState } from 'react';
import { Grid, Typography } from '@mui/material';
import MainCard from 'components/MainCard';
import LinearWithLabel from 'components/@extended/progress/LinearWithLabel';
import { GetQualificationsByUser, Qualification } from 'api/qualificationService';
import { BookOutlined } from '@ant-design/icons';
const MyQualificationsProfile: React.FC = () => {

  const [qualifications, setQualifications] = useState<Qualification[]>([]);

  useEffect(() => {
    const fetchQualifications = async () => {
        const response = await GetQualificationsByUser();
        console.log(response)
        setQualifications(response);
    }

    fetchQualifications();
  }, []);

  return (
    <MainCard title="My Qualifications">
      <Grid container spacing={1.25}>
        {qualifications && (
          qualifications.map((data, index) => (
            <React.Fragment key={index}>
              <Grid item xs={2}>
                <BookOutlined />
              </Grid>
              <Grid item xs={10}>
                <Typography color="secondary">{data.name}</Typography>
              </Grid>
            </React.Fragment>
          ))
        )}
      </Grid>
    </MainCard>
  );
};

export default MyQualificationsProfile;
