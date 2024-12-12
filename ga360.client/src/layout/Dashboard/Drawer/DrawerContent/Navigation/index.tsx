import { useContext, useLayoutEffect, useState } from 'react';

// material-ui
import { Theme } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';
import Box from '@mui/material/Box';
import Divider from '@mui/material/Divider';
import List from '@mui/material/List';
import Typography from '@mui/material/Typography';

// project import
import NavItem from './NavItem';
import NavGroup from './NavGroup';
import menuItem, { getMenuItemsByRole } from 'menu-items';
import { MenuFromAPI } from 'menu-items/dashboard';

import useConfig from 'hooks/useConfig';
import { HORIZONTAL_MAX_ITEM, MenuOrientation } from 'config';
import { useGetMenu, useGetMenuMaster } from 'api/menu';

// types
import { NavItemType } from 'types/menu';
import DuendeContext from 'contexts/DuendeContext';



// ==============================|| DRAWER CONTENT - NAVIGATION ||============================== //

export default function Navigation() {
  const { menuOrientation } = useConfig();
  const { menuLoading } = useGetMenu();
  const { menuMaster } = useGetMenuMaster();
  const drawerOpen = menuMaster.isDashboardDrawerOpened;
  const downLG = useMediaQuery((theme: Theme) => theme.breakpoints.down('lg'));

  const [selectedID, setSelectedID] = useState<string | undefined>('');
  const [selectedItems, setSelectedItems] = useState<string | undefined>('');
  const [selectedLevel, setSelectedLevel] = useState<number>(0);
  const [menuItems, setMenuItems] = useState<{ items: NavItemType[] }>({ items: [] });

  const { user, isLoggedIn } = useContext(DuendeContext);

  //let dashboardMenu = MenuFromAPI();

// const dashboardMenu: NavItemType = {
//   id: 'group-dashboards',
//   title: <FormattedMessage id="Dashboards" />,
//   icon: icons.PieChartOutlined,
//   type: 'group',
//   children: [
//     {
//       id: 'group-dashboards-collapse',
//       title: <FormattedMessage id="Principal" />,
//       type: 'collapse',
//       icon: icons.PieChartOutlined,
//       children: [
//         {
//           id: 'group-dashboards-backoffice',
//           title: <FormattedMessage id="Back Office" />,
//           type: 'item',
//           url: '/charts/apexchart'
//         },
//       ]
//     },
//   ]
// };

  useLayoutEffect(() => {
    // const isFound = menuItem.items.some((element) => {
    //   if (element.id === 'group-dashboard') {
    //     return true;
    //   }
    //   return false;
    // });

    // if (menuLoading) {
    //   menuItem.items.splice(0, 0, dashboardMenu);
    //   setMenuItems({ items: [...menuItem.items] });
    // } else if (!menuLoading && dashboardMenu?.id !== undefined && !isFound) {
    //   menuItem.items.splice(0, 1, dashboardMenu);
    //   setMenuItems({ items: [...menuItem.items] });
    // } else {
    //   setMenuItems({ items: [...menuItem.items] });
    // }
    console.log("OH USER", user)
    console.log("MENU ITEMS",menuItem.items )
    setMenuItems({ items: [...menuItem.items] });

    if (user && user.role) {
      const updatedMenuItems = getMenuItemsByRole(user.role);
      setMenuItems(updatedMenuItems);
    }

    // eslint-disable-next-line
  }, [menuLoading]);

  // const filterMenuItems = (role, menuItems) => {
  //   switch (role) {
  //     case 'Training Centre':
  //       return menuItems.items.filter(item => 
  //         item.id === 'group-dashboards-collapse' || 
  //         item.id === 'group-back-office' ||
  //         item.id === 'group-back-office-collapse'
  //       );
  //     case 'Super Admin':
  //       return menuItems.items; // All items for Super Admin
  //     case 'Candidate':
  //       return menuItems.items.filter(item => item.id === 'group-profile');
  //     case 'Assessor':
  //       return menuItems.items.filter(item => item.id === 'group-profile');
  //     default:
  //       return [];
  //   }
  // };
  
  // // Example usage in useLayoutEffect
  // useLayoutEffect(() => {
  //   if (user && user.role) {
  //     console.log("user",user)

  //     const filteredMenuItems = filterMenuItems(user.role, menuItems);
  //     console.log("MENU ITEMS", filteredMenuItems)
  //     //setMenuItems({ items: filteredMenuItems });
  //     setMenuItems({ items: [...menuItem.items] })
  //   }
  // }, [menuLoading]);
  

  const isHorizontal = menuOrientation === MenuOrientation.HORIZONTAL && !downLG;

  const lastItem = isHorizontal ? HORIZONTAL_MAX_ITEM : null;
  let lastItemIndex = menuItems.items.length - 1;
  let remItems: NavItemType[] = [];
  let lastItemId: string;

  //  first it checks menu item is more than giving HORIZONTAL_MAX_ITEM after that get lastItemid by giving horizontal max
  // item and it sets horizontal menu by giving horizontal max item lastly slice menuItem from array and set into remItems

  if (lastItem && lastItem < menuItems.items.length) {
    lastItemId = menuItems.items[lastItem - 1].id!;
    lastItemIndex = lastItem - 1;
    remItems = menuItems.items.slice(lastItem - 1, menuItems.items.length).map((item) => ({
      title: item.title,
      elements: item.children,
      icon: item.icon,
      ...(item.url && {
        url: item.url
      })
    }));
  }

  const navGroups = menuItems.items.slice(0, lastItemIndex + 1).map((item, index) => {
    switch (item.type) {
      case 'group':
        if (item.url && item.id !== lastItemId) {
          return (
            <List key={item.id} {...(isHorizontal && { sx: { mt: 0.5 } })}>
              {!isHorizontal && index !== 0 && <Divider sx={{ my: 0.5 }} />}
              <NavItem item={item} level={1} isParents />
            </List>
          );
        }

        return (
          <NavGroup
            key={item.id}
            setSelectedID={setSelectedID}
            setSelectedItems={setSelectedItems}
            setSelectedLevel={setSelectedLevel}
            selectedLevel={selectedLevel}
            selectedID={selectedID}
            selectedItems={selectedItems}
            lastItem={lastItem!}
            remItems={remItems}
            lastItemId={lastItemId}
            item={item}
          />
        );
      default:
        return (
          <Typography key={item.id} variant="h6" color="error" align="center">
            Fix - Navigation Group
          </Typography>
        );
    }
  });

  return (
    <Box
      sx={{
        pt: drawerOpen ? (isHorizontal ? 0 : 2) : 0,
        ...(!isHorizontal && { '& > ul:first-of-type': { mt: 0 } }),
        display: isHorizontal ? { xs: 'block', lg: 'flex' } : 'block'
      }}
    >
      {navGroups}
    </Box>
  );
}
