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
import GA360LogoIcon2 from './GA360LogoIcon2';
import { Typography } from '@mui/material';
import { ReactSVG } from 'react-svg';
import GA360LogoPNG from './GA360Logo_colour.png'
import GA360LogoPNGWhite from './GA360Logo_White.png'
import { padding } from '@mui/system';

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
    {isIcon ? <div style={{paddingLeft:'2.5em'}}><img src={GA360LogoPNGWhite} alt="Global Alliance 360" style={{ width: '50px'}}/></div>:<img src={GA360LogoPNG} alt="Global Alliance 360" style={{ width: '110px'}}/> }
      {/* {isIcon ? <LogoIcon /> : <Logo reverse={reverse} />} */}
    </ButtonBase>
    {isIcon ?<></>:
    <Typography variant="caption" display="block" align="left" sx={{ mt: 1.5, fontSize: '0.45rem' }}>
        v1.0.4 Alpha-9/1/2025
    </Typography>}
    </div>
  );
}