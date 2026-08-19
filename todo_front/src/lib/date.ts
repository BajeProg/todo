const dateFormatter = new Intl.DateTimeFormat('ru-RU', {
  day: 'numeric',
  month: 'short',
  year: 'numeric',
});

export const formatDate = (value: string): string => dateFormatter.format(new Date(value));

export const isOverdue = (value: string): boolean => {
  const deadline = new Date(value);
  deadline.setHours(23, 59, 59, 999);
  return deadline.getTime() < Date.now();
};

export const toDateInput = (value: string | null): string => value ? value.slice(0, 10) : '';

export const toApiDeadline = (value: string): string | null => {
  if (!value) return null;
  return new Date(`${value}T23:59:59`).toISOString();
};
