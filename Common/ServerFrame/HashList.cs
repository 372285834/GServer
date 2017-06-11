using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerFrame
{
    /// <summary>
    /// 哈希列表;
    /// </summary>
    public class HashList<T> : IDisposable, ISubject
    {
        //哈希表对象，用来进行键值数据处理;
        public Hashtable hash = new Hashtable();
        //泛型列表，用来进行有序数据处理;
        public List<T> list = new List<T>();
        //字符串格式化标志;
        //true 打印list;
        //false 打印hashTabel;
        public bool typeList = true;


        ///////////////////////////////////////  public method  ////////////////////////////////////////////////////////////


        //////////////////////////////////// 以下三个代码是实现观察者模式;  ////////////////////////////////////////////

        //观察者列表;
        protected List<IObserver> observer = new List<IObserver>();

        /// <summary>
        /// 注册观察者;
        /// </summary>
        public void AddObserver(IObserver value)
        {
            observer.Add(value);
        }

        /// <summary>
        /// 移除观察者;
        /// </summary>
        public void RemoveObserver(IObserver value)
        {
            observer.Remove(value);
        }

        /// <summary>
        /// 通知;
        /// </summary>
        public void Change(object[] param = null)
        {
            int length = observer.Count;
            for (int i = 0; i < length; i++)
            {
                IObserver value = observer[i];
                value.OnChanged(param);
            }
        }

        /// <summary>
        /// 获取一个Array结构的数据集;
        /// </summary>
        /// <returns></returns>
        public T[] array
        {
            get
            {
                T[] array = new T[count];
                list.CopyTo(array);
                return array;
            }
        }

        /// <summary>
        /// 从当前列表中指定的索引位置处截取num个对象，并返回;
        /// </summary>
        /// <param name="start">截取的起始索引</param>
        /// <param name="num">截取数量</param>
        /// <returns></returns>
        public List<T> splice(int start, int num)
        {
            T[] array = new T[num];
            list.CopyTo(start, array, 0, num);
            removeFromHashByArray(array);
            return array.ToList();
        }

        /// <summary>
        /// 连接两个hashList;
        /// </summary>
        /// <param name="value"></param>
        public void concat(HashList<T> value)
        {
            list = list.Concat(value.list).ToList();
            addFromHash(value.hash);
        }

        /// <summary>
        /// 在每一个元素上执行函数，函数返回类型为空;
        /// </summary>
        /// <param name="action"></param>
        public void forEach(Action<T> action)
        {
            list.ForEach(action);
        }

        /// <summary>
        /// 通过键值添加对象;
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void addElement(object key, T value)
        {
            list.Add(value);
            hash.Add(key, value);
        }

        /// <summary>
        /// 向指定的索引处添加对象;
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public void addElement(object key, T value, int index)
        {
            list.Insert(index, value);
            hash.Add(key, value);
        }

        /// <summary>
        /// 设置指定键值的值;
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void setElementByKey(object key, T value)
        {
            T old = (T)hash[key];
            hash[key] = value;
            int index = list.IndexOf(old);
            list[index] = value;
        }

        /// <summary>
        /// 获取指定索引处的对象;
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T getElementByIndex(int index)
        {
            return list.ElementAtOrDefault(index);
        }

        /// <summary>
        /// 获取指定键值的对象;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T getElementByKey(object key)
        {
            if (hash.ContainsKey(key))
            {
                return (T)hash[key];
            }
            return default(T);
        }

        /// <summary>
        /// 删除指定的对象;
        /// </summary>
        /// <param name="value"></param>
        public void removeElement(T value)
        {
            list.Remove(value);
            removeFromHash(value);
        }

        /// <summary>
        /// 删除指定键值的对象;
        /// </summary>
        /// <param name="key"></param>
        public void removeElementByKey(object key)
        {
            T value = (T)hash[key];
            list.Remove(value);
            hash.Remove(key);
        }

        /// <summary>
        /// 删除指定索引处的对象;
        /// </summary>
        /// <param name="index"></param>
        public void removeElementByIndex(int index)
        {
            T value = list.ElementAtOrDefault(index);
            list.Remove(value);
            removeFromHash(value);
        }

        /// <summary>
        /// 获取list中的最后一个对象，并将其从list中删除;
        /// </summary>
        /// <returns></returns>
        public T pop()
        {
            if (list.Count > 0)
            {
                int index = list.Count - 1;
                T value = list.ElementAt(index);
                removeElementByIndex(index);
                return value;
            }
            return default(T);
        }

        /// <summary>
        /// 获取list中的第一个对象，并将其从list中删除;
        /// </summary>
        /// <returns></returns>
        public T shift()
        {
            T value = list.ElementAt(0);
            removeElementByIndex(0);
            return value;
        }

        /// <summary>
        /// 根据指定的接口排序类，对list进行排序，并将排序后的list返回;
        /// </summary>
        /// <param name="compare"></param>
        /// <returns></returns>
        public List<T> sort(IComparer<T> compare)
        {
            list.Sort(compare);
            return list;
        }

        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的元素，并返回整个 List<T> 中的第一个匹配元素;
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public T find(Predicate<T> match)
        {
            return list.Find(match);
        }

        /// <summary>
        /// 检索与指定谓词定义的条件匹配的所有元素;
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<T> findAll(Predicate<T> match)
        {
            return list.FindAll(match);
        }

        /// <summary>
        /// 返回指定对象的索引;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int indexOf(T value)
        {
            return list.IndexOf(value);
        }

        /// <summary>
        /// 清空列表中所有数据;
        /// </summary>
        public void clear()
        {
            list.Clear();
            hash.Clear();
        }

        /// <summary>
        /// 将列表转化成字符串;
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
//             if (typeList)
//                 return Json.toString(list);
//             else
//                 return Json.toString(hash);
            return null;
        }

        /// <summary>
        /// 获取list中包含的元素数;
        /// </summary>
        public int count
        {
            get
            {
                return list.Count;
            }
        }

        /// <summary>
        /// 释放;
        /// </summary>
        public void Dispose()
        {
            Dispose(true);//释放所有的资源;
            GC.SuppressFinalize(this);//不需要再调用本对象的Finalize方法
        }
        ///////////////////////////////// protected method ///////////////////////////////////////////////////////

        /// <summary>
        /// 从hash表中删除指定对象;
        /// </summary>
        /// <param name="value"></param>
        protected void removeFromHash(T value)
        {
            object key = null;
            foreach (DictionaryEntry de in hash)
            {

                if (de.Value != null && de.Value.Equals(value))
                {
                    key = de.Key;
                    break;
                }
            }
            if (key != null)
                hash.Remove(key);
        }

        /// <summary>
        /// 从指定的数组中删除对象;
        /// </summary>
        /// <param name="array"></param>
        protected void removeFromHashByArray(T[] array)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
                removeElement(array[i]);
        }

        /// <summary>
        /// 释放对象;
        /// </summary>
        /// <param name="disposing">是否清理托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            clear();
            //清理托管资源;
            if (disposing)
            {
                list = null;
                hash = null;
            }
        }

        /// <summary>
        /// 从指定的hash表中添加数据;
        /// </summary>
        /// <param name="value"></param>
        protected void addFromHash(Hashtable value)
        {
            foreach (DictionaryEntry de in value)
            {
                hash.Add(de.Key, de.Value);
            }
        }
    }
}
