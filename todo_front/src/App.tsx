import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { CheckSquare2, Folder, FolderPlus, Inbox, Menu, Pencil, Plus, Search, Trash2, X } from 'lucide-react';
import { useMemo, useState } from 'react';
import { projectsApi } from './api/projects';
import { tasksApi } from './api/tasks';
import { ErrorNotice } from './components/ErrorNotice';
import { Modal } from './components/Modal';
import { ProjectForm } from './components/ProjectForm';
import { TaskCard } from './components/TaskCard';
import { TaskForm } from './components/TaskForm';
import type { Project, ProjectInput, TaskInput, TaskItem } from './domain/todo';

type DialogState =
  | { type: 'create-project' }
  | { type: 'edit-project'; project: Project }
  | { type: 'create-task' }
  | { type: 'edit-task'; task: TaskItem }
  | null;

const getMessage = (error: unknown) => error instanceof Error ? error.message : 'Неизвестная ошибка';
const EMPTY_PROJECTS: Project[] = [];
const EMPTY_TASKS: TaskItem[] = [];

export default function App() {
  const queryClient = useQueryClient();
  const [selectedProjectId, setSelectedProjectId] = useState<string | null>(null);
  const [search, setSearch] = useState('');
  const [dialog, setDialog] = useState<DialogState>(null);
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const [actionError, setActionError] = useState<string | null>(null);

  const projectsQuery = useQuery({ queryKey: ['projects'], queryFn: projectsApi.getAll });
  const tasksQuery = useQuery({ queryKey: ['tasks'], queryFn: tasksApi.getAll });

  const refresh = () => Promise.all([
    queryClient.invalidateQueries({ queryKey: ['projects'] }),
    queryClient.invalidateQueries({ queryKey: ['tasks'] }),
  ]);

  const projectMutation = useMutation({
    mutationFn: ({ id, input }: { id?: string; input: ProjectInput }) => id ? projectsApi.update(id, input) : projectsApi.create(input),
    onSuccess: async () => { await refresh(); setDialog(null); },
  });
  const taskMutation = useMutation({
    mutationFn: ({ id, input }: { id?: string; input: TaskInput }) => id ? tasksApi.update(id, input) : tasksApi.create(input),
    onSuccess: async () => { await queryClient.invalidateQueries({ queryKey: ['tasks'] }); setDialog(null); },
  });
  const deleteProjectMutation = useMutation({
    mutationFn: projectsApi.remove,
    onSuccess: async (_, id) => {
      if (selectedProjectId === id) setSelectedProjectId(null);
      await refresh();
    },
    onError: (error) => setActionError(getMessage(error)),
  });
  const deleteTaskMutation = useMutation({
    mutationFn: tasksApi.remove,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['tasks'] }),
    onError: (error) => setActionError(getMessage(error)),
  });

  const projects = projectsQuery.data ?? EMPTY_PROJECTS;
  const tasks = tasksQuery.data ?? EMPTY_TASKS;
  const selectedProject = projects.find((project) => project.id === selectedProjectId);
  const filteredTasks = useMemo(() => {
    const needle = search.trim().toLocaleLowerCase('ru');
    return tasks.filter((task) => {
      if (selectedProjectId && task.project.id !== selectedProjectId) return false;
      if (!needle) return true;
      return [task.name, task.description, task.project.name].some((value) => value?.toLocaleLowerCase('ru').includes(needle));
    });
  }, [search, selectedProjectId, tasks]);

  const selectProject = (id: string | null) => {
    setSelectedProjectId(id);
    setSidebarOpen(false);
  };

  const handleDeleteProject = (project: Project) => {
    setActionError(null);
    if (window.confirm(`Удалить проект «${project.name || 'Без названия'}»? Связанные задачи также могут быть удалены.`)) {
      deleteProjectMutation.mutate(project.id);
    }
  };
  const handleDeleteTask = (task: TaskItem) => {
    setActionError(null);
    if (window.confirm(`Удалить задачу «${task.name || 'Без названия'}»?`)) deleteTaskMutation.mutate(task.id);
  };

  const isLoading = projectsQuery.isPending || tasksQuery.isPending;
  const loadError = projectsQuery.error || tasksQuery.error;

  return (
    <div className="app-shell">
      <aside className={sidebarOpen ? 'sidebar sidebar--open' : 'sidebar'}>
        <div className="brand"><span className="brand__mark"><CheckSquare2 size={20} /></span><span>точка</span></div>
        <button className="sidebar__close icon-button" type="button" aria-label="Закрыть меню" onClick={() => setSidebarOpen(false)}><X size={20} /></button>
        <nav className="project-nav" aria-label="Проекты">
          <div className="project-nav__heading"><span>Пространство</span><button className="icon-button icon-button--small" type="button" aria-label="Создать проект" onClick={() => { projectMutation.reset(); setDialog({ type: 'create-project' }); }}><FolderPlus size={17} /></button></div>
          <button className={!selectedProjectId ? 'nav-item nav-item--active' : 'nav-item'} type="button" onClick={() => selectProject(null)}>
            <span><Inbox size={18} /> Все задачи</span><small>{tasks.length}</small>
          </button>
          {projects.map((project, index) => (
            <div className={selectedProjectId === project.id ? 'nav-item nav-item--active' : 'nav-item'} key={project.id}>
              <button className="nav-item__select" type="button" onClick={() => selectProject(project.id)}>
                <span className={`project-dot project-dot--${index % 4}`} /><span>{project.name || 'Без названия'}</span>
              </button>
              <span className="nav-item__count">{tasks.filter((task) => task.project.id === project.id).length}</span>
              <span className="nav-item__actions">
                <button type="button" aria-label="Редактировать проект" onClick={() => { projectMutation.reset(); setDialog({ type: 'edit-project', project }); }}><Pencil size={14} /></button>
                <button type="button" aria-label="Удалить проект" onClick={() => handleDeleteProject(project)}><Trash2 size={14} /></button>
              </span>
            </div>
          ))}
        </nav>
        <div className="sidebar__footer"><span className="status-dot" /> API подключён</div>
      </aside>
      {sidebarOpen && <button className="sidebar-scrim" aria-label="Закрыть меню" onClick={() => setSidebarOpen(false)} />}

      <main className="main">
        <header className="topbar">
          <button className="mobile-menu icon-button" type="button" aria-label="Открыть меню" onClick={() => setSidebarOpen(true)}><Menu size={21} /></button>
          <label className="search"><Search size={18} /><input value={search} onChange={(event) => setSearch(event.target.value)} placeholder="Найти задачу…" aria-label="Поиск задач" /></label>
          <button className="button button--primary button--compact" type="button" disabled={!projects.length} onClick={() => { taskMutation.reset(); setDialog({ type: 'create-task' }); }}><Plus size={18} /> <span>Новая задача</span></button>
        </header>

        <div className="content">
          <div className="page-heading">
            <div><span className="eyebrow">{selectedProject ? 'Проект' : 'Рабочий обзор'}</span><h1>{selectedProject?.name || 'Все задачи'}</h1><p>{selectedProject?.description || 'Соберите важное в одном месте и двигайтесь по плану.'}</p></div>
            <div className="task-count"><strong>{filteredTasks.length}</strong><span>{filteredTasks.length === 1 ? 'задача' : 'задач'}</span></div>
          </div>

          {actionError && <ErrorNotice message={actionError} />}
          {loadError && <ErrorNotice message={getMessage(loadError)} onRetry={() => { void projectsQuery.refetch(); void tasksQuery.refetch(); }} />}

          {isLoading ? (
            <div className="task-grid" aria-label="Загрузка задач">{[1, 2, 3].map((item) => <div className="task-card skeleton" key={item}><span /><span /><span /></div>)}</div>
          ) : filteredTasks.length ? (
            <section className="task-grid" aria-label="Список задач">
              {filteredTasks.map((task) => <TaskCard key={task.id} task={task} onEdit={(item) => { taskMutation.reset(); setDialog({ type: 'edit-task', task: item }); }} onDelete={handleDeleteTask} />)}
            </section>
          ) : (
            <div className="empty-state">
              <span className="empty-state__icon">{search ? <Search size={27} /> : <Folder size={27} />}</span>
              <h2>{search ? 'Ничего не найдено' : projects.length ? 'Здесь пока тихо' : 'Начните с проекта'}</h2>
              <p>{search ? 'Попробуйте изменить запрос или выбрать другой проект.' : projects.length ? 'Добавьте первую задачу — и она появится здесь.' : 'Проект поможет сгруппировать задачи и держать фокус.'}</p>
              {!search && <button className="button button--primary" type="button" onClick={() => projects.length ? setDialog({ type: 'create-task' }) : setDialog({ type: 'create-project' })}><Plus size={18} /> {projects.length ? 'Добавить задачу' : 'Создать проект'}</button>}
            </div>
          )}
        </div>
      </main>

      {dialog?.type === 'create-project' && <Modal title="Новый проект" eyebrow="Организуйте работу" onClose={() => setDialog(null)}><ProjectForm isSaving={projectMutation.isPending} error={projectMutation.error ? getMessage(projectMutation.error) : undefined} onSubmit={(input) => projectMutation.mutate({ input })} onCancel={() => setDialog(null)} /></Modal>}
      {dialog?.type === 'edit-project' && <Modal title="Редактировать проект" eyebrow="Настройки проекта" onClose={() => setDialog(null)}><ProjectForm project={dialog.project} isSaving={projectMutation.isPending} error={projectMutation.error ? getMessage(projectMutation.error) : undefined} onSubmit={(input) => projectMutation.mutate({ id: dialog.project.id, input })} onCancel={() => setDialog(null)} /></Modal>}
      {dialog?.type === 'create-task' && <Modal title="Новая задача" eyebrow="Следующий шаг" onClose={() => setDialog(null)}><TaskForm projects={projects} initialProjectId={selectedProjectId ?? undefined} isSaving={taskMutation.isPending} error={taskMutation.error ? getMessage(taskMutation.error) : undefined} onSubmit={(input) => taskMutation.mutate({ input })} onCancel={() => setDialog(null)} /></Modal>}
      {dialog?.type === 'edit-task' && <Modal title="Редактировать задачу" eyebrow="Детали задачи" onClose={() => setDialog(null)}><TaskForm task={dialog.task} projects={projects} isSaving={taskMutation.isPending} error={taskMutation.error ? getMessage(taskMutation.error) : undefined} onSubmit={(input) => taskMutation.mutate({ id: dialog.task.id, input })} onCancel={() => setDialog(null)} /></Modal>}
    </div>
  );
}
