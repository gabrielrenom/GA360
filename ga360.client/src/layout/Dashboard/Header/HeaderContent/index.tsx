import { useContext, useMemo } from 'react';

// material-ui
import { Theme } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';
import Box from '@mui/material/Box';

// project import
import Search from './Search';
import Message from './Message';
import Profile from './Profile';
import Localization from './Localization';
import Notification from './Notification';
import FullScreen from './FullScreen';
import Customization from './Customization';
import MobileSection from './MobileSection';
import MegaMenuSection from './MegaMenuSection';

import useConfig from 'hooks/useConfig';
import { MenuOrientation } from 'config';
import DrawerHeader from 'layout/Dashboard/Drawer/DrawerHeader';
import GA360LogoIcon from 'components/logo/GA360LogoIcon';
import DuendeContext from 'contexts/DuendeContext';

// ==============================|| HEADER - CONTENT ||============================== //

export default function HeaderContent() {
  const { menuOrientation } = useConfig();

  const downLG = useMediaQuery((theme: Theme) => theme.breakpoints.down('lg'));

  const localization = useMemo(() => <Localization />, []);

  const megaMenu = useMemo(() => <MegaMenuSection />, []);
  const { user, isLoggedIn } = useContext(DuendeContext);

  return (
    <>
      {menuOrientation === MenuOrientation.HORIZONTAL && !downLG && <DrawerHeader open={true} />}
      {user && user.role === 'Candidate'?
      <Box
      sx={{
        paddingTop:'1em',
        width: '70px', // set the desired width
        height: '70px', // set the desired height
        '& svg': {
          width: '70%', // scale the SVG to the parent's width
          height: '70%', // scale the SVG to the parent's height
        },
      }}
    >
      <GA360LogoIcon />
    </Box>:<></>}
      {!downLG && <Search />}
      {/* {!downLG && megaMenu} */}
      {/* {!downLG && localization} */}
      {/* {downLG && <Box sx={{ width: '100%', ml: 1 }} />} */}

      {/* <Notification /> */}
      {/* <Message /> */}
      {!downLG && <FullScreen />}
      {/* <Customization /> */}
      {!downLG && <Profile />}
      {downLG && <MobileSection />}
    </>
  );
}
