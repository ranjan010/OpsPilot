export const ROUTE_PATHS = {
  login: 'login',
  register: 'register',
  home: '',
  orders: 'orders',
  orderDetails: 'order-details'
} as const;

export const ROUTE_HIERARCHY = {
  home: {
    path: ROUTE_PATHS.home,
    children: {
      orders: {
        path: ROUTE_PATHS.orders,
        children: {
          orderDetails: {
            path: ROUTE_PATHS.orderDetails
          }
        }
      }
    }
  }
} as const;

export function buildRoutePath(...segments: Array<string | undefined>): string {
  return segments.filter((segment): segment is string => Boolean(segment && segment.length > 0)).join('/');
}

export function buildRouteLink(...segments: Array<string | undefined>): string {
  const route = buildRoutePath(...segments);
  return route ? `/${route}` : '/';
}
