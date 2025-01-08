import { Link } from 'react-router-dom';
import { To } from 'history';

// material-ui
import { SxProps } from '@mui/material/styles';
import ButtonBase from '@mui/material/ButtonBase';

// project import
import Logo from './LogoMain';
import LogoIcon from './LogoIcon';
import { APP_DEFAULT_PATH } from 'config';
import useAuth from 'hooks/useAuth';
import GA360LogoIcon from './GA360LogoIcon';
import { Typography } from '@mui/material';

// ==============================|| MAIN LOGO ||============================== //

interface Props {
  reverse?: boolean;
  isIcon?: boolean;
  sx?: SxProps;
  to?: To;
}

export default function LogoSection({ reverse, isIcon, sx, to }: Props) {
  const { isLoggedIn } = useAuth();

  return (
    <div style ={{paddingRight:'13px', paddingTop:'0.9rem'}}>
    <ButtonBase disableRipple {...(isLoggedIn && { component: Link, to: !to ? APP_DEFAULT_PATH : to, sx })}>
      {isIcon ? <GA360LogoIcon /> : <Logo reverse={reverse} />}
      {/* {isIcon ? <LogoIcon /> : <Logo reverse={reverse} />} */}
    </ButtonBase>
    {isIcon ?<></>:
    <Typography variant="caption" display="block" align="left" sx={{ mt: 1.5, fontSize: '0.45rem' }}>
        v1.0.3 Alpha-8/1/2025
    </Typography>}
    </div>
  );
}