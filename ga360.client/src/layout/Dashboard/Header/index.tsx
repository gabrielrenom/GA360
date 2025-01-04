import { ReactNode, useContext, useMemo } from 'react';

// material-ui
import { useTheme } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';
import AppBar, { AppBarProps } from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';

// project import
import AppBarStyled from './AppBarStyled';
import HeaderContent from './HeaderContent';
import IconButton from 'components/@extended/IconButton';

import useConfig from 'hooks/useConfig';
import { handlerDrawerOpen, useGetMenuMaster } from 'api/menu';
import { MenuOrientation, ThemeMode, DRAWER_WIDTH, MINI_DRAWER_WIDTH } from 'config';

// assets
import MenuFoldOutlined from '@ant-design/icons/MenuFoldOutlined';
import MenuUnfoldOutlined from '@ant-design/icons/MenuUnfoldOutlined';
import Logo from 'components/logo';
import DuendeContext from 'contexts/DuendeContext';

// ==============================|| MAIN LAYOUT - HEADER ||============================== //

export default function Header() {
  const theme = useTheme();
  const downLG = useMediaQuery(theme.breakpoints.down('lg'));
  const { mode, menuOrientation } = useConfig();

  const { menuMaster } = useGetMenuMaster();
  const drawerOpen = menuMaster.isDashboardDrawerOpened;

  const isHorizontal = menuOrientation === MenuOrientation.HORIZONTAL && !downLG;

  // header content
  const headerContent = useMemo(() => <HeaderContent />, []);
  const { user, isLoggedIn } = useContext(DuendeContext);

  const iconBackColor = mode === ThemeMode.DARK ? 'background.default' : 'grey.100';

  // common header
  const mainHeader: ReactNode = (
    <Toolbar>
      {user && user.role !== "Candidate" ? (
        !isHorizontal ? (
          <IconButton
            aria-label="open drawer"
            onClick={() => handlerDrawerOpen(!drawerOpen)}
            edge="start"
            color="secondary"
            variant="light"
            sx={{
              color: 'text.primary',
              bgcolor: drawerOpen ? 'transparent' : iconBackColor,
              ml: { xs: 0, lg: -2 },
            }}
          >
            {!drawerOpen ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
          </IconButton>
        ) : null
      ) : null}
      {headerContent}
    </Toolbar>
  );
  

  // app-bar params with drawer
  const appBar: AppBarProps = {
    position: 'fixed',
    color: 'inherit',
    elevation: 0,
    sx: {
      borderBottom: '1px solid',
      borderBottomColor: 'divider',
      zIndex: 1200,
      width: isHorizontal
        ? '100%'
        : { xs: '100%', lg: drawerOpen ? `calc(100% - ${DRAWER_WIDTH}px)` : `calc(100% - ${MINI_DRAWER_WIDTH}px)` }
    }
  };

    // app-bar params full width
    const appBarFullWidth: AppBarProps = {
      position: 'fixed',
      color: 'inherit',
      elevation: 0,
      sx: {
        borderBottom: '1px solid',
        borderBottomColor: 'divider',
        zIndex: 1200,
       paddingTop:'0.1em'
      }
    };

  return (
    <>
    {user && user.role === 'Candidate'?
    <AppBar {...appBarFullWidth}>{mainHeader}</AppBar>
    :<>
        {!downLG ? (
        <AppBarStyled open={drawerOpen} {...appBar}>
          {mainHeader}
        </AppBarStyled>
      ) : (
        <AppBar {...appBar}>{mainHeader}</AppBar>
      )}
    </>
      }
    </>
  );
}
