import { describe, expect, it } from 'vitest';
import { toApiDeadline, toDateInput } from './date';

describe('date helpers', () => {
  it('converts an API timestamp to a date input value', () => {
    expect(toDateInput('2026-08-19T12:30:00.000Z')).toBe('2026-08-19');
    expect(toDateInput(null)).toBe('');
  });

  it('returns null for an optional empty deadline', () => {
    expect(toApiDeadline('')).toBeNull();
  });

  it('creates an ISO timestamp for a selected local date', () => {
    expect(toApiDeadline('2026-08-19')).toMatch(/^2026-08-19T/);
  });
});
