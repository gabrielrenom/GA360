import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { To } from 'history';
import { SxProps, ButtonBase, Typography } from '@mui/material';
import useAuth from 'hooks/useAuth';
import { getTrainingCentre, TrainingCentre } from 'api/trainingcentreService';
import GA360LogoPNG from './GA360Logo_colour.png';
import System360LogoPNG from './SYSTEM360_Logo_Dark.png';
import GA360LogoPNGWhite from './GA360Logo_White.png';
import { APP_DEFAULT_PATH } from 'config';
import { version } from '../../../package.json';

interface Props {
  reverse?: boolean;
  isIcon?: boolean;
  sx?: SxProps;
  to?: To;
}

const LogoSection: React.FC<Props> = ({ reverse, isIcon, sx, to }) => {
  const { isLoggedIn, user } = useAuth();
  const [trainingCentre, setTrainingCentre] = useState<TrainingCentre | null>(null);

  useEffect(() => {
    const fetchTrainingCentreData = async () => {
      try {
        if (user.trainingCentreId) {
          const data = await getTrainingCentre(user.trainingCentreId);
          setTrainingCentre(data);
        }
      } catch (error) {
        console.error('Failed to fetch training centre data:', error);
      }
    };

    fetchTrainingCentreData();
  }, [user.trainingCentreId]);

  if (user.role !== 'Super Admin') {
    return (
      <div style={{ paddingRight: '13px', paddingTop: '0.9rem' }}>
        <ButtonBase disableRipple {...(isLoggedIn && { component: Link, to: !to ? '/' : to, sx })}>
          {isIcon ? (
            <div style={{ paddingLeft: '2.5em' }}>
              {trainingCentre?.logo && (
                                <div style={{paddingRight:'17px'}}>
                <img src={trainingCentre.logo} alt={trainingCentre.name} style={{ width: '40px' }} />
                </div>
              )}
            </div>
          ) : (
            <>
              {trainingCentre?.logo && (
                <img src={trainingCentre.logo} alt={trainingCentre.name} style={{ width: '160px' }} />
              )}
            </>
          )}
        </ButtonBase>
        {!isIcon && (
          <Typography variant="caption" display="block" align="left" sx={{ mt: 1.5, fontSize: '0.45rem' }}>
            {version}
          </Typography>
        )}
      </div>
    );
  } else {
    return (
      <div style={{ paddingRight: '13px', paddingTop: '0.9rem' }}>
        <ButtonBase disableRipple {...(isLoggedIn && { component: Link, to: !to ? APP_DEFAULT_PATH : to, sx })}>
          {isIcon ? (
            <div style={{ paddingLeft: '2.5em' }}>
              <img src={GA360LogoPNGWhite} alt="Global Alliance 360" style={{ width: '50px' }} />
            </div>
          ) : (
            <img src={System360LogoPNG} alt="Global Alliance 360" style={{ width: '160px' }} />
          )}
        </ButtonBase>
        {!isIcon && (
          <Typography variant="caption" display="block" align="left" sx={{ pt: 1.8,ml: 1, fontSize: '0.45rem' }}>
            {version}
          </Typography>
        )}
      </div>
    );
  }
};

export default LogoSection;
