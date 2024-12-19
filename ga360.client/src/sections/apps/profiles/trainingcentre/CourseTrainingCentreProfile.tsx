import React, { useEffect, useState } from 'react';
import { Grid, Typography } from '@mui/material';
import MainCard from 'components/MainCard';
import LinearWithLabel from 'components/@extended/progress/LinearWithLabel';
import { CourseModel } from 'types/customerApiModel';
import { getCandidate } from 'api/customer';
import { CustomerListExtended } from 'types/customer';




const CourseProgressions: React.FC = () => {
  const [candidate, setCandidate] = useState<CustomerListExtended>(null);

  useEffect(() => {
    const fetchUser = async () => {
        try {
            const response = await getCandidate();
            setCandidate(response);
        } catch (error) {
            console.error("Error fetching countries:", error);
        }
    };

    fetchUser();
}, []);

  return (
    <MainCard title="Course Progressions">
      <Grid container spacing={1.25}>
        {candidate !== null && candidate.courses !== null ? (
          candidate.courses.map((data, index) => (
            <React.Fragment key={index}>
              <Grid item xs={6}>
                <Typography color="secondary">{data.name}</Typography>
              </Grid>
              <Grid item xs={6}>
                <LinearWithLabel value={data.progression} />
              </Grid>
            </React.Fragment>
          ))
        ) : (
          <></>
        )}
      </Grid>
    </MainCard>
  );
};

export default CourseProgressions;
