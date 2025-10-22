export interface Toast {
  id: string;
  message: string;
  type: 'success' | 'error' | 'warning' | 'info';
  duration?: number;
}

let toasts: Toast[] = [];
let listeners: Array<(toasts: Toast[]) => void> = [];

export function addToast(toast: Omit<Toast, 'id'>) {
  const id = Math.random().toString(36).substr(2, 9);
  const newToast: Toast = {
    id,
    duration: 4000,
    ...toast
  };

  toasts = [...toasts, newToast];
  notifyListeners();

  // Auto remove after duration
  setTimeout(() => {
    removeToast(id);
  }, newToast.duration);

  return id;
}

export function removeToast(id: string) {
  toasts = toasts.filter(t => t.id !== id);
  notifyListeners();
}

export function clearAllToasts() {
  toasts = [];
  notifyListeners();
}

export function subscribe(listener: (toasts: Toast[]) => void) {
  listeners = [...listeners, listener];
  return () => {
    listeners = listeners.filter(l => l !== listener);
  };
}

function notifyListeners() {
  listeners.forEach(listener => listener([...toasts]));
}

// Convenience functions
export function showSuccess(message: string, duration?: number) {
  return addToast({ message, type: 'success', duration });
}

export function showError(message: string, duration?: number) {
  return addToast({ message, type: 'error', duration });
}

export function showWarning(message: string, duration?: number) {
  return addToast({ message, type: 'warning', duration });
}

export function showInfo(message: string, duration?: number) {
  return addToast({ message, type: 'info', duration });
}
