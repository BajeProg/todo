import { AlertCircle, RotateCcw } from 'lucide-react';

interface ErrorNoticeProps {
  message: string;
  onRetry?: () => void;
}

export function ErrorNotice({ message, onRetry }: ErrorNoticeProps) {
  return (
    <div className="error-notice" role="alert">
      <AlertCircle size={20} aria-hidden="true" />
      <div>
        <strong>Что-то пошло не так</strong>
        <p>{message}</p>
      </div>
      {onRetry && (
        <button className="text-button" type="button" onClick={onRetry}>
          <RotateCcw size={15} /> Повторить
        </button>
      )}
    </div>
  );
}
