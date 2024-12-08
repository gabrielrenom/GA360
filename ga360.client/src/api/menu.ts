import useSWR, { mutate } from 'swr';
import { useMemo } from 'react';

// Project-imports
import { fetcher } from 'utils/axios';

// types
import { MenuProps, NavItemType } from 'types/menu';

const initialState: MenuProps = {
  isDashboardDrawerOpened: false,
  isComponentDrawerOpened: true
};

export const endpoints = {
  key: 'api/menu',
  master: 'master',
  dashboard: '/dashboard' // server URL
};

export function useGetMenu() {
  //UNCOMMENT TO COME TO THE ORIGINAL

  // const { data, isLoading, error, isValidating } = useSWR(endpoints.key + endpoints.dashboard, fetcher, {
  //   revalidateIfStale: false,
  //   revalidateOnFocus: false,
  //   revalidateOnReconnect: false
  // });

  // const memoizedValue = useMemo(
  //   () => ({
  //     menu: data?.dashboard as NavItemType,
  //     menuLoading: isLoading,
  //     menuError: error,
  //     menuValidating: isValidating,
  //     menuEmpty: !isLoading && !data?.length
  //   }),
  //   [data, error, isLoading, isValidating]
  // );
  // console.log("MEMORIZE")
  // console.log(memoizedValue)
  const memoizedValue = {
    menuLoading: true,
    menuValidating: false,
    menuEmpty: false
};
  return memoizedValue;
}

export function useGetMenuMaster() {
  const { data, isLoading } = useSWR(endpoints.key + endpoints.master, () => initialState, {
    revalidateIfStale: false,
    revalidateOnFocus: false,
    revalidateOnReconnect: false
  });

  const memoizedValue = useMemo(
    () => ({
      menuMaster: data as MenuProps,
      menuMasterLoading: isLoading
    }),
    [data, isLoading]
  );

  return memoizedValue;
}

export function handlerComponentDrawer(isComponentDrawerOpened: boolean) {
  // to update local state based on key

  mutate(
    endpoints.key + endpoints.master,
    (currentMenuMaster: any) => {
      return { ...currentMenuMaster, isComponentDrawerOpened };
    },
    false
  );
}

export function handlerDrawerOpen(isDashboardDrawerOpened: boolean) {
  // to update local state based on key

  mutate(
    endpoints.key + endpoints.master,
    (currentMenuMaster: any) => {
      return { ...currentMenuMaster, isDashboardDrawerOpened };
    },
    false
  );
}
